using Autofac;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;
using Doc2web.Core;
using System.IO;

namespace Doc2web
{
    /// <summary>
    /// Object that converts open xml elements into html.
    /// </summary>
    public class ConversionEngine : IDisposable
    {
        private ILifetimeScope _lifetimeScope;
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
            SetupProcessors(plugins);
            SetupLifetimeScopeFromScratch();
            SetupConversionTaskFactory();
        }

        /// <summary>
        /// Creates a new conversion engine.
        /// </summary>
        /// <param name="plugins">Instances of classes that have methods with attributes that register 
        /// hooks(InitializeEngineAttribute,PreProcessingAttribute, ElementProcessingAttribute and PostProcessingAttribute).</param>
        public ConversionEngine(ILifetimeScope parentScope, params object[] plugins)
        {
            SetupProcessors(plugins);
            SetupLifetimeScopeFromParent(parentScope);
            SetupConversionTaskFactory();
        }

        private void SetupProcessors(object[] plugins)
        {
            _processorFactory = new ProcessorFactory();
            _processor = _processorFactory.BuildMultiple(plugins);
        }

        private void SetupLifetimeScopeFromScratch()
        {
            var containerBuilder = new ContainerBuilder();
            _processor.InitEngine(containerBuilder);
            _lifetimeScope = containerBuilder.Build();
        }
        private void SetupLifetimeScopeFromParent(ILifetimeScope parentScope)
        {
            _lifetimeScope = parentScope.BeginLifetimeScope(_processor.InitEngine);
        }

        private void SetupConversionTaskFactory()
        {
            _conversionTaskFactory = new ConversionTaskFactory
            {
                LifetimeScope = _lifetimeScope,
                ProcessorFactory = _processorFactory,
                Processor = _processor,
                ContextRenderer = new Core.Rendering.ContextRenderer()
            };
        }

        /// <summary>
        /// Dispose the conversion engine and it's IoC container.
        /// </summary>
        public void Dispose()
        {
            _lifetimeScope.Dispose();
        }

        public void Convert(ConversionParameter parameter)
        {
            var conversionTask = _conversionTaskFactory.Build(parameter);
            ExecuteConversionTask(conversionTask);
        }

        public string ConvertToString(StringConversionParameter parameter)
        {
            var tempPlugin = _processorFactory.BuildMultiple(parameter.AdditionalPlugins);
            var conversionTask = _conversionTaskFactory.Build(parameter);
            ExecuteConversionTask(conversionTask);
            return parameter.GetResult();
        }

        private static void ExecuteConversionTask(IConversionTask conversionTask)
        {
            conversionTask.PreProcess();
            conversionTask.ProcessElements();
            conversionTask.PostProcess();
            conversionTask.AssembleDocument();
        }
    }
}
