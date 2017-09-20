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
        //private static string[] restricted = new string[]
        //{
        //    //"Bold",
        //    //"Caps",
        //    //"Color",
        //    //"FontSize",
        //    //"Highlight",
        //    //"Italic",
        //    //"Justification",
        //    //"RunFonts",
        //    //"SmallCaps",
        //    //"Underline"
        //};

        private StyleConfiguration _config = new StyleConfiguration();

        public string ParagraphClassCssPrefix
        {
            get => _config.ParagraphCssClassPrefix;
            set => _config.ParagraphCssClassPrefix = value;
        }

        public string RunClassCssPrefix
        {
            get => _config.RunCssClassPrefix;
            set => _config.RunCssClassPrefix = value;
        }

        public StyleProcessorPlugin(WordprocessingDocument wpDoc)
        {
            _wpDoc = wpDoc;
            ParagraphClassCssPrefix = "div.container";
            RunClassCssPrefix = "span";
            _config.LeftIdentationCssClassPrefix = "> .leftspacer";
            _config.RightIdentationCssClassPrefix = "> .rightspacer";
        }

        private Styles Styles => _wpDoc.MainDocumentPart.StyleDefinitionsPart.Styles;
        private Theme Theme => _wpDoc.MainDocumentPart.ThemePart.Theme;

        [InitializeEngine]
        public void InitEngine(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("CssProperty") && !t.IsAbstract)
                //.Where(t =>
                //{
                //    foreach (var r in restricted)
                //        if (t.Name.StartsWith(r)) return false;
                //    return true;
                //})
                .As(x => {
                    var t = typeof(CssProperty<>);
                    return t.MakeGenericType(x.BaseType.GetGenericArguments());
                });
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
                    r.Resolve<StyleConfiguration>(),
                    r.Resolve<ICssPropertiesFactory>(),
                    r.Resolve<INumberingCrawler>()))
                .As<ICssClassFactory>()
                .InstancePerLifetimeScope();
        }

        [InitializeProcessing]
        public void InitProcessing(ContainerBuilder builder)
        {
            builder.RegisterInstance(_config.Clone());
            builder
                .RegisterType<CssRegistrator>()
                .As<ICssRegistrator>()
                .InstancePerLifetimeScope();
        }

        [PostProcessing]
        public void InjectCss(IGlobalContext context)
        {
            var cssRegistrator = context.Container.Resolve<ICssRegistrator>() as CssRegistrator;
            var sb = new StringBuilder(".container { display: flex; } .container > p { flex: 1;  margin-top: 0; }");
            cssRegistrator.RenderInto(sb);
            context.AddCss(sb.ToString());
        }
    }
}
