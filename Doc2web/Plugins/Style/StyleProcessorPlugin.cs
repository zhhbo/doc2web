using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Text;
using Doc2web;
using Autofac;

namespace Doc2web.Plugins.Style
{
    public class StyleProcessorPlugin
    {
        public StyleProcessorPlugin(WordprocessingDocument wpDoc)
        {

        }

        [PreProcessing]
        public void RegisterCssManager(IGlobalContext context, ContainerBuilder builder)
        {

        }
    }
}
