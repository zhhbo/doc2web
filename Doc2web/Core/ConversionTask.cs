using Autofac;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Core
{
    public class ConversionTask : IConversionTask
    {
        private StringBuilder _result;

        public ConversionTask()
        {
            _result = new StringBuilder();
        }

        public string Result => _result.ToString();

        public IGlobalContext GlobalContext { get; set; }

        public IProcessor Processor { get; set; }

        public IContextRenderer ContextRenderer { get; set; }

        public void Initialize()
        {
            GlobalContext.Container =
                GlobalContext.Container.BeginLifetimeScope(containerBuilder =>
                    Processor.InitProcess(containerBuilder));
        }

        public void PreProcess()
        {
            Processor.PreProcess(GlobalContext);
        }

        public void ConvertElements() {
            var tasks =
                GlobalContext
                .RootElements
                .Select(elem => Task.Factory.StartNew(() => ConvertRootElement(elem)))
                .ToArray();

            for(int i = 0; i < tasks.Length; i++)
            {
                tasks[i].Wait();
                _result.AppendLine(tasks[i].Result);
            }
        }


        public void PostProcess()
        {
            Processor.PostProcess(GlobalContext);
        }

        public void AssembleDocument()
        {
            AddCss();
            AddJs();
            _result.AppendLine(GlobalContext.Html);
        }

        private string ConvertRootElement(OpenXmlElement rootElement)
        {
            var context = new RootElementContext(GlobalContext, rootElement);
            var task = new ElementProcessingTask(context, Processor);
            task.Execute();
            return ContextRenderer.Render(context);

        }

        private void AddCss()
        {
            _result.AppendLine(@"<style>");
            _result.AppendLine(GlobalContext.Css);
            _result.AppendLine(@"</style>");
        }
        private void AddJs()
        {
            _result.AppendLine(@"<script>");
            _result.AppendLine(GlobalContext.Js);
            _result.AppendLine(@"</script>");
        }
    }
}
