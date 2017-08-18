using Autofac;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whusion.Core
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

        public void PreProcess()
        {
            GlobalContext.Container = 
                GlobalContext.Container.BeginLifetimeScope(containerBuilder =>
                    Processor.PreProcess(GlobalContext, containerBuilder));
        }

        public void ConvertElements() {
            var htmlElements =
                    GlobalContext
                    .RootElements
                    .AsParallel()
                    .Select(ConvertElement)
                    .ToArray();

            foreach (var htmlElement in htmlElements)
                _result.AppendLine(htmlElement);
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

        private string ConvertElement(OpenXmlElement element)
        {
            var elementContext = new ElementContext(GlobalContext, element);
            Processor.ProcessElement(elementContext, element);
            return ContextRenderer.Render(elementContext);

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
