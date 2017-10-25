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
using WStyle = DocumentFormat.OpenXml.Wordprocessing.Style;
using Doc2web.Plugins.Style.Css;

namespace Doc2web.Plugins.Style
{
    /// <summary>
    /// Add CssRegistrator in the IoC container and render the css in post-processing.
    /// </summary>
    public class StylePlugin
    {
        private WordprocessingDocument _wpDoc;

        private StyleConfig _config;

        public StylePlugin(WordprocessingDocument wpDoc, StyleConfig config)
        {
            _wpDoc = wpDoc;
            _config = config;
        }

        public StylePlugin(WordprocessingDocument wpDoc) : this(wpDoc, new StyleConfig()) { }

        [InitializeEngine]
        public void InitEngine(ContainerBuilder builder)
        {
            // Starting point, wpDoc disceted
            RegisterWpDocConfig(builder);

            // Providers
            RegisterProviders(builder);

            // Props and Prop factory
            RegisterCssProps(builder);

            // Caches
            RegisterCache(builder);

            // Class factories
            RegisterClassFactories(builder);

            // Registrator
            RegisterApi(builder);
        }

        private static void RegisterApi(ContainerBuilder builder)
        {
            builder
                .RegisterType<CssRegistrator>()
                .As<ICssRegistrator>()
                .InstancePerLifetimeScope();
        }

        private static void RegisterClassFactories(ContainerBuilder builder)
        {
            builder.RegisterType<ClsNameGenerator>()
                .SingleInstance();
            builder.Register(c =>
                new ParagraphClassFactory(
                    c.Resolve<StyleConfig>(),
                    c.Resolve<ClsNameGenerator>(),
                    c.Resolve<IDefaultsProvider>(),
                    c.Resolve<PStylePPropsCache>(),
                    c.Resolve<ContainerNumberingPropsCache>(),
                    c.Resolve<Func<CssPropertySource, ICssPropertiesFactory>>()))
                .As<IParagraphClassFactory>()
                .SingleInstance();
            builder.Register(c =>
                new RunClassFactory(
                    c.Resolve<StyleConfig>(),
                    c.Resolve<ClsNameGenerator>(),
                    c.Resolve<IDefaultsProvider>(),
                    c.Resolve<PStyleRPropsCache>(),
                    c.Resolve<NumberNumberingPropsCache>(),
                    c.Resolve<RStyleRPropsCache>(),
                    c.Resolve<Func<CssPropertySource, ICssPropertiesFactory>>()))
                .As<IRunClassFactory>();
        }

        private static void RegisterCache(ContainerBuilder builder)
        {
            builder.RegisterTypes(
                    typeof(PStylePPropsCache),
                    typeof(PStyleRPropsCache),
                    typeof(RStyleRPropsCache),
                    typeof(ContainerNumberingPropsCache),
                    typeof(NumberNumberingPropsCache))
                .SingleInstance();
        }

        private void RegisterCssProps(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.Namespace.StartsWith("Doc2web.Plugins.Style"))
                .Where(HasCssProperytAttribute)
                .WithMetadataFrom<BaseCssPropertyAttribute>()
                .As<ICssProperty>();
            builder
                .RegisterType<CssPropertiesFactory>()
                .As<ICssPropertiesFactory>();
        }

        private static void RegisterProviders(ContainerBuilder builder)
        {
            builder
                .Register(r => new ThemeColorsProvider(r.Resolve<Theme>()))
                .As<IThemeColorsProvider>()
                .SingleInstance();
            builder
                .Register(r => new ThemeFontsProvider(r.Resolve<Theme>()))
                .As<IThemeFontsProvider>()
                .SingleInstance();
            builder.Register(r => new NumberingProvider(
                    r.Resolve<WordprocessingDocument>()
                    .MainDocumentPart?.NumberingDefinitionsPart?.Numbering,
                    r.Resolve<WordprocessingDocument>()
                    .MainDocumentPart?.StyleDefinitionsPart?.Styles))
                .As<INumberingProvider>()
                .SingleInstance();
            builder
                .RegisterType<DefaultsProvider>()
                .As<IDefaultsProvider>()
                .SingleInstance();
        }

        private void RegisterWpDocConfig(ContainerBuilder builder)
        {
            builder
                .RegisterInstance(_config);
            builder
                .RegisterInstance(_wpDoc)
                .ExternallyOwned();
            builder
                .Register(c =>
                    c.Resolve<WordprocessingDocument>()
                    .MainDocumentPart.StyleDefinitionsPart.Styles)
                .ExternallyOwned();
            builder
                .Register(c => c.Resolve<Styles>().Elements<WStyle>());
            builder
                .Register(c =>
                    c.Resolve<WordprocessingDocument>()
                    .MainDocumentPart.ThemePart.Theme)
                .ExternallyOwned();
        }

        private bool HasCssProperytAttribute(Type arg) =>
            arg.CustomAttributes.Where(x =>
                x.AttributeType.BaseType == typeof(BaseCssPropertyAttribute))
            .Any();

        [PostProcessing]
        public void InjectCss(IGlobalContext context)
        {
            var cssRegistrator = context.Resolve<ICssRegistrator>();
            var data = new CssData();
            var sb = new StringBuilder();

            cssRegistrator.InsertCss(data);
            data.RenderInto(sb);
            context.AddCss(BaseCss);
            context.AddCss(sb.ToString());
        }

        public string BaseCss =>
            $"{_config.ContainerCssClassPrefix} {{ display: flex; }} " +
            $"{_config.ContainerCssClassPrefix} > {_config.ParagraphCssClassPrefix} {{ flex: 1;  margin: 0; }}";
    }
}
