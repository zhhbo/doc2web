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
    public class StylePlugin
    {
        private WordprocessingDocument _wpDoc;

        private StyleConfig _config = new StyleConfig();

        public StylePlugin(WordprocessingDocument wpDoc)
        {
            _wpDoc = wpDoc;
        }

        private Styles Styles => _wpDoc.MainDocumentPart.StyleDefinitionsPart.Styles;
        private Theme Theme => _wpDoc.MainDocumentPart.ThemePart.Theme;

        [InitializeEngine]
        public void InitEngine(ContainerBuilder builder)
        {
            builder.RegisterInstance(_config);
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("CssProperty") && !t.IsAbstract && t.Namespace.StartsWith("Doc2web.Plugins.Style"))
                .As(x => x.AsRegistrableCssProperty());
            builder
                .Register(r => new ThemeColorsProvider(Theme))
                .As<IThemeColorsProvider>()
                .InstancePerLifetimeScope();
            builder
                .Register(r => new ThemeFontsProvider(Theme))
                .As<IThemeFontsProvider>()
                .InstancePerLifetimeScope();
            builder
                .RegisterType<CssPropertiesFactory>()
                .As<ICssPropertiesFactory>()
                .As<ICssPropertiesFactory>();
            builder.RegisterInstance(new NumberingCrawler(
                _wpDoc.MainDocumentPart.NumberingDefinitionsPart.Numbering,
                _wpDoc.MainDocumentPart.StyleDefinitionsPart.Styles))
                .As<INumberingCrawler>();
            builder
                .Register(r => new CssClassFactory(
                    Styles, 
                    r.Resolve<StyleConfig>(),
                    r.Resolve<ICssPropertiesFactory>(),
                    r.Resolve<INumberingCrawler>()))
                .As<ICssClassFactory>()
                .InstancePerLifetimeScope();
            builder
                .RegisterType<CssRegistrator>()
                .As<ICssRegistrator>()
                .InstancePerLifetimeScope();
        }

        [PostProcessing]
        public void InjectCss(IGlobalContext context)
        {
            var cssRegistrator = context.Resolve<ICssRegistrator>() as CssRegistrator;
            var sb = new StringBuilder(".container { display: flex; } .container > p { flex: 1;  margin-top: 0; }");
            cssRegistrator.RenderInto(sb);
            context.AddCss(sb.ToString());
        }
    }
}
