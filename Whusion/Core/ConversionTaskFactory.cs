using Autofac;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;
using Whusion.Core.Rendering;

namespace Whusion.Core
{
    public class ConversionTaskFactory
    {
        public IContainer EngineContainer { get; set; }

        public IProcessor Processor { get; set; }

        public IContextRenderer ContextRenderer { get; set; }

        public IConversionTask Build(IEnumerable<OpenXmlElement> elements)
        {
            var container = EngineContainer.BeginLifetimeScope();
            return new ConversionTask
            {
                GlobalContext = new GlobalContext(container, elements),
                Processor = Processor,
                ContextRenderer = ContextRenderer
            };
        }
    }
}
