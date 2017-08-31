using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Text;
using Doc2web;
using Autofac;
using System.Reflection;
using Doc2web.Plugins.Style.Properties;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Plugins.Style
{
    public class StyleProcessorPlugin
    {
        private WordprocessingDocument _wpDoc;

        public StyleProcessorPlugin(WordprocessingDocument wpDoc)
        {
            _wpDoc = wpDoc;
        }

        [InitializeEngine]
        public void RegisterCssManager(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("CssProperty") && t.Name != "BaseCssProperty")
                .As(x => {
                    var t = typeof(BaseCssProperty<>);
                    return t.MakeGenericType(x.GetGenericArguments());
                });
            builder
                .RegisterType<CssPropertiesFactory>()
                .As<ICssPropertiesFactory>();
            builder
                .RegisterType<CssRegistrator>()
                .InstancePerLifetimeScope()
                .As<ICssRegistrator>();
        }
    }
}
