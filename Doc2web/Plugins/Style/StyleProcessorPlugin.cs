using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Text;
using Doc2web;
using Autofac;
using System.Reflection;
using Doc2web.Plugins.Style.Properties;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Drawing;

namespace Doc2web.Plugins.Style
{
    public class StyleProcessorPlugin
    {
        private WordprocessingDocument _wpDoc;

        public StyleProcessorPlugin(WordprocessingDocument wpDoc)
        {
            _wpDoc = wpDoc;
        }

        private Styles Styles => _wpDoc.MainDocumentPart.StyleDefinitionsPart.Styles;
        private Theme Theme => _wpDoc.MainDocumentPart.ThemePart.Theme; 

        [InitializeEngine]
        public void InitEngine(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("CssProperty") && !t.IsAbstract)
                .As(x => {
                    var t = typeof(BaseCssProperty<>);
                    return t.MakeGenericType(x.BaseType.GetGenericArguments());
                });
            builder
                .Register(r => new ThemeColorProvider(Theme))
                .As<IThemeColorProvider>()
                .InstancePerLifetimeScope();
            builder
                .RegisterType<CssPropertiesFactory>()
                .As<ICssPropertiesFactory>()
                .As<ICssPropertiesFactory>();
            builder
                .Register(r => new CssClassFactory(Styles, r.Resolve<ICssPropertiesFactory>()))
                .As<ICssClassFactory>()
                .InstancePerLifetimeScope();
        }

        [InitializeProcessing]
        public void InitProcessing(ContainerBuilder builder)
        {
            builder
                .RegisterType<CssRegistrator>()
                .As<ICssRegistrator>()
                .InstancePerLifetimeScope();
        }
        
        [PostProcessing]
        public void InjectCss(IGlobalContext context)
        {
            var cssRegistrator = context.Container.Resolve<ICssRegistrator>() as CssRegistrator;
            var sb = new StringBuilder();
            cssRegistrator.RenderInto(sb);
            context.AddCss(sb.ToString());
        }
    }
}
