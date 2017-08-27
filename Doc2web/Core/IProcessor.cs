using Autofac;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Core
{
    public interface IProcessor
    {
        void InitProcess(ContainerBuilder containerBuilder);
        void PreProcess(IGlobalContext context);
        void ProcessElement(IElementContext context, OpenXmlElement element);
        void PostProcess(IGlobalContext context);
    }
}
