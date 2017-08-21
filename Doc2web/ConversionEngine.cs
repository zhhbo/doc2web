using Autofac;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;
using Doc2web.Core;

namespace Doc2web
{
    public class ConversionEngine : IDisposable
    {
        private ConversionTaskFactory _conversionTaskFactory;
        private Processor _processor;

        public ConversionEngine(params object[] plugins)
        {
            _processor = new ProcessorFactory().BuildMultiple(plugins);
            _conversionTaskFactory = new ConversionTaskFactory();
            _conversionTaskFactory.EngineContainer = new ContainerBuilder().Build();
            _conversionTaskFactory.Processor = _processor;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public string Render(IEnumerable<OpenXmlElement> elements)
        {
            var conversionTask = _conversionTaskFactory.Build(elements);

            conversionTask.PreProcess();
            conversionTask.ConvertElements();
            conversionTask.PostProcess();
            conversionTask.AssembleDocument();

            return conversionTask.Result;
        }
    }
}
