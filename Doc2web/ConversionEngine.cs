using Autofac;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;
using Doc2web.Core;

namespace Doc2web
{
    /// <summary>
    /// Object that converts open xml elements into html.
    /// </summary>
    public class ConversionEngine : IDisposable
    {
        private IContainer _container;
        private ConversionTaskFactory _conversionTaskFactory;
        private ProcessorFactory _processorFactory;
        private Processor _processor;

        /// <summary>
        /// Creates a new conversion engine.
        /// </summary>
        /// <param name="plugins">Instances of classes that have methods with attributes that register 
        /// hooks(InitializeEngineAttribute,PreProcessingAttribute, ElementProcessingAttribute and PostProcessingAttribute).</param>
        public ConversionEngine(params object[] plugins)
        {
            _processorFactory = new ProcessorFactory();
            _processor = _processorFactory.BuildMultiple(plugins);
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

        /// <summary>
        /// Dispose the conversion engine and it's IoC container.
        /// </summary>
        public void Dispose()
        {
            _container.Dispose();
        }

        /// <summary>
        /// Convert some open xml elements in HTML.
        /// </summary>
        /// <param name="elements">Targeted open xml elements.</param>
        /// <returns>HTML produce by the conversion engine.</returns>
        public string Convert(IEnumerable<OpenXmlElement> elements)
        {
            var conversionTask = _conversionTaskFactory.Build(elements);

            return ExecuteConversionTask(conversionTask);
        }

        /// <summary>
        /// Convert some open xml elements in HTML.
        /// Add some temporary plugins for this conversion task.
        /// </summary>
        /// <param name="elements">Targeted open xml elements.</param>
        /// <param name="temporaryPlugins">Plugins that will be added for this conversion task.</param>
        /// <returns>HTML produce by the conversion engine.</returns>
        public string Convert(IEnumerable<OpenXmlElement> elements, params object[] temporaryPlugins)
        {
            var tempPlugin = _processorFactory.BuildMultiple(temporaryPlugins);
            var conversionTask = _conversionTaskFactory.Build(elements, tempPlugin);

            return ExecuteConversionTask(conversionTask);
        }

        private static string ExecuteConversionTask(IConversionTask conversionTask)
        {
            conversionTask.PreProcess();
            conversionTask.ProcessElements();
            conversionTask.PostProcess();
            conversionTask.AssembleDocument();

            return conversionTask.Result;
        }
    }
}
