﻿using Autofac;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whusion.Core
{
    public interface IProcessor
    {
        void PreProcess(IGlobalContext context, ContainerBuilder containerBuilder);
        void ProcessElement(IElementContext context, OpenXmlElement element);
        void PostProcess(IGlobalContext context);
    }
}
