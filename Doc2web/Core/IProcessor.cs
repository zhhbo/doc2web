using Autofac;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Core
{
    public interface IProcessor
    {
        void PreProcess(IGlobalContext context, ContainerBuilder containerBuilder);
        void ProcessElement(Doc2web.IElementContext context, OpenXmlElement element);
        void PostProcess(IGlobalContext context);
    }
}
