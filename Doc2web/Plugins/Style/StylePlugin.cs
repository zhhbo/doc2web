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
using System.Linq;

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
            builder
                .RegisterInstance(Styles)
                .ExternallyOwned();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.Namespace.StartsWith("Doc2web.Plugins.Style"))
                .Where(HasCssProperytAttribute)
                .WithMetadataFrom<BaseCssPropertyAttribute>()
                .As<ICssProperty>();
            builder
                .Register(r => new ThemeColorsProvider(Theme))
                .As<IThemeColorsProvider>()
                .InstancePerLifetimeScope();
            builder
                .Register(r => new ThemeFontsProvider(Theme))
                .As<IThemeFontsProvider>()
                .InstancePerLifetimeScope();
            builder
                .RegisterType<CssPropertiesFactory2>()
                .As<ICssPropertiesFactory2>()
                .As<ICssPropertiesFactory2>();
            builder.RegisterInstance(new NumberingCrawler(
                _wpDoc.MainDocumentPart.NumberingDefinitionsPart.Numbering,
                _wpDoc.MainDocumentPart.StyleDefinitionsPart.Styles))
                .As<INumberingCrawler>();
            builder
                .RegisterType<CssClassFactory>()
                .As<ICssClassFactory>()
                .SingleInstance();
            builder
                .RegisterType<CssRegistrator>()
                .As<ICssRegistrator>()
                .InstancePerLifetimeScope();
        }

        private bool HasCssProperytAttribute(Type arg) =>
            arg.CustomAttributes.Where(x =>
                x.AttributeType.BaseType == typeof(BaseCssPropertyAttribute))
            .Any();

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
