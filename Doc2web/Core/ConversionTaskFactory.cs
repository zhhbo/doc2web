using Autofac;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;
using Doc2web.Core.Rendering;

namespace Doc2web.Core
{
    public class ConversionTaskFactory
    {
        public IContainer EngineContainer { get; set; }

        public IProcessor Processor { get; set; }

        public IContextRenderer ContextRenderer { get; set; }

        public IConversionTask Build(IEnumerable<OpenXmlElement> elements)
        {
            return new ConversionTask
            {
                Processor = Processor,
                RootElements = elements,
                Container = EngineContainer,
                ContextRenderer = ContextRenderer
            };
        }
    }
}
