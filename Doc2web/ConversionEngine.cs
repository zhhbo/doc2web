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
        private IContainer _container;
        private ConversionTaskFactory _conversionTaskFactory;
        private Processor _processor;

        public ConversionEngine(params object[] plugins)
        {
            _processor = new ProcessorFactory().BuildMultiple(plugins);
            Initialize();
        }

        private void Initialize()
        {
            var containerBuilder = new ContainerBuilder();
            _processor.InitEngine(containerBuilder);
            _container = containerBuilder.Build();

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
