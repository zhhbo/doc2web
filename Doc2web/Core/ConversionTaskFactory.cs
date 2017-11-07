using Autofac;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;
using Doc2web.Core.Rendering;
using System.IO;

namespace Doc2web.Core
{
    public class ConversionTaskFactory
    {
        public ILifetimeScope LifetimeScope { get; set; }

        public Processor Processor { get; set; }

        public IContextRenderer ContextRenderer { get; set; }

        public IProcessorFactory ProcessorFactory { get; set; }

        public IConversionTask Build(ConversionParameter parameter)
        {
            Processor processor; 
            if (parameter.AdditionalPlugins.Count > 0)
            {
                processor = new Processor(
                    Processor, 
                    ProcessorFactory.BuildMultiple(parameter.AdditionalPlugins)
                );
            } else
            {
                processor = Processor;
            }

            return new ConversionTask
            {
                Processor = processor,
                RootElements = parameter.Elements,
                LifetimeScope = LifetimeScope,
                ContextRenderer = ContextRenderer,
                Out = new StreamWriter(parameter.Stream) { AutoFlush = parameter.AutoFlush }
            };
        }
    }
}
