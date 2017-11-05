using Autofac;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Core
{
    public class ConversionTask : IConversionTask
    {
        public ConversionTask() { }

        public StreamWriter Out { get; set; }

        public IEnumerable<OpenXmlElement> RootElements { get; set; }

        public IContainer Container { get; set; }

        public IGlobalContext GlobalContext { get; set; }

        public IProcessor Processor { get; set; }

        public IContextRenderer ContextRenderer { get; set; }

        public void PreProcess()
        {
            GlobalContext = new GlobalContext(Container.BeginLifetimeScope(), RootElements);
            Processor.PreProcess(GlobalContext);
        }

        public void ProcessElements()
        {
            var tasks =
                GlobalContext
                .RootElements
                .Select(ProcessRootElement)
                .ToArray();

            Task.WaitAll(tasks);
        }

        public void PostProcess()
        {
            Processor.PostProcess(GlobalContext);
        }

        public void AssembleDocument()
        {
            var renderingTasks =
                GlobalContext
                .RootElements
                .Select(RenderElementContext)
                .ToArray();

            Out.Write(Part1);
            Out.Write(GlobalContext.Css);
            Out.Write(Part2);


            for(int i = 0; i < renderingTasks.Length; i++)
            {
                var task = renderingTasks[i];
                task.Wait();
                QueueWrite(task.Result);
            }

            _writtingTask.Wait();
            Out.Write(GlobalContext.Html);
            Out.Write(Part3);
            Out.Write(GlobalContext.Js);
            Out.Write(Part4);
            Out.Flush();
        }

        private void QueueWrite(string data)
        {
            if (_writtingTask != null) _writtingTask.Wait();
            _writtingTask = Out.WriteAsync(data);
        }

        private Task<string> RenderElementContext(IElementContext context)
        {
            return Task.Factory.StartNew(state => {
                string x = ContextRenderer.Render((IElementContext)state);
                return x;
            }, context);
        }

        private static string Part1 = @"<!DOCTYPE html><html><head><style>";
        private static string Part2 = @"</style></head><body>";
        private static string Part3 = @"<script>";
        private static string Part4 = @"</script></body></html>";
        private Task _writtingTask;

        private Task<RootElementContext> ProcessRootElement(RootElementContext rootElementContext)
        {
            return Task.Factory.StartNew((state) =>
            {
                var context = (RootElementContext)state;
                var task = new ElementProcessingTask(context, Processor);
                task.Execute();
                return context;
            }, rootElementContext);
        }
    }
}
