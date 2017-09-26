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
        private StringBuilder _elementBuffer;

        public ConversionTask()
        {
            _elementBuffer = new StringBuilder();
        }

        public string Result => new string(_buffer);

        public IEnumerable<OpenXmlElement> RootElements { get; set; }

        public IContainer Container { get; set; }

        public IGlobalContext GlobalContext { get; set; }

        public IProcessor Processor { get; set; }

        public IContextRenderer ContextRenderer { get; set; }

        public void Initialize()
        {
            GlobalContext = new GlobalContext(
                Container.BeginLifetimeScope(Processor.InitProcess),
                RootElements);
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
                _elementBuffer.AppendLine(tasks[i].Result);
            }
        }


        public void PostProcess()
        {
            Processor.PostProcess(GlobalContext);
        }

        public void AssembleDocument()
        {
            _buffer = new char[RequiredSpace];
            int i = 0;

            i = CopyToBuffer(i, Part1);
            i = CopyToBuffer(i, GlobalContext.Css);
            i = CopyToBuffer(i, Part2);

            _elementBuffer.CopyTo(0, _buffer, i, _elementBuffer.Length);
            i += _elementBuffer.Length;

            i = CopyToBuffer(i, GlobalContext.Html);
            i = CopyToBuffer(i, Part3);
            i = CopyToBuffer(i, GlobalContext.Js);
            i = CopyToBuffer(i, Part4);
        }

        private int RequiredSpace => 
                GlobalContext.Css.Length +
                GlobalContext.Js.Length +
                GlobalContext.Html.Length +
                Part1.Length +
                Part2.Length +
                Part3.Length + 
                Part4.Length +
                _elementBuffer.Length;

        private static string Part1 = @"<!DOCTYPE html><html><head><style>";
        private static string Part2 = @"</style></head><body>";
        private static string Part3 = @"<script>";
        private static string Part4 = @"</script></body></html>";
        private char[] _buffer;

        private int CopyToBuffer(int i, string value)
        {
            value.CopyTo(0, _buffer, i, value.Length);
            return i + value.Length;
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
            _elementBuffer.AppendLine(@"<style>");
            _elementBuffer.AppendLine(GlobalContext.Css);
            _elementBuffer.AppendLine(@"</style>");
        }
        private void AddJs()
        {
            _elementBuffer.AppendLine(@"<script>");
            _elementBuffer.AppendLine(GlobalContext.Js);
            _elementBuffer.AppendLine(@"</script>");
        }
    }
}
