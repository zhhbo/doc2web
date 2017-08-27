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
        private IContainer _container;
        private Processor _processor;

        public ConversionEngine(params object[] plugins)
        {
            _container = new ContainerBuilder().Build();
            _processor = new ProcessorFactory().BuildMultiple(plugins);
            _conversionTaskFactory = new ConversionTaskFactory
            {
                EngineContainer = _container,
                Processor = _processor,
                ContextRenderer = new Core.Rendering.ContextRenderer()
            };
        }

        public void Dispose()
        {
            _container.Dispose();
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
