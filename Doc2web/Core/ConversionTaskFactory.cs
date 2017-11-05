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

        public IConversionTask Build(IEnumerable<OpenXmlElement> elements, Stream stream)
        {
            return new ConversionTask
            {
                Processor = Processor,
                RootElements = elements,
                LifetimeScope = LifetimeScope,
                ContextRenderer = ContextRenderer,
                Out = new StreamWriter(stream)
            };
        }

        public IConversionTask Build(IEnumerable<OpenXmlElement> elements, Stream stream, Processor temporary)
        {
            var processor = new Processor(Processor, temporary);
            return new ConversionTask
            {
                Processor = processor,
                RootElements = elements,
                LifetimeScope = LifetimeScope,
                ContextRenderer = ContextRenderer,
                Out = new StreamWriter(stream)
            };
        }
    }
}
