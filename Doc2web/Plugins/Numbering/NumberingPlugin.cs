using Autofac;
using Doc2web.Plugins.Numbering.Mapping;
using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Css;
using Doc2web.Plugins.Style.Properties;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Doc2web.Plugins.Numbering
{
    public class NumberingPlugin
    {
        private WordprocessingDocument _wpDoc;
        private NumberingConfig _config;

        public NumberingPlugin(WordprocessingDocument wpDoc) : this(wpDoc, new NumberingConfig()) { }

        public NumberingPlugin(WordprocessingDocument wpDoc, NumberingConfig config)
        {
            _wpDoc = wpDoc;
            _config = config;
        }

        [InitializeEngine]
        public void InitEngine(ContainerBuilder builder)
        {
            builder
                .RegisterInstance(_config);
            builder
                .RegisterInstance(_wpDoc)
                .ExternallyOwned();
            builder
                .RegisterType<NumberingMapper>()
                .As<INumberingMapper>()
                .SingleInstance();
            builder
                .RegisterTypes(typeof(NumberingIndentationCssProperty))
                .WithMetadataFrom<BaseCssPropertyAttribute>()
                .As<ICssProperty>();
        }

        [ElementProcessing]
        public void InsertNumbering(IElementContext context, Paragraph p)
        {
            var numberingMapper = context.Resolve<INumberingMapper>();

            if (!numberingMapper.IsValid) return;

            var numbering = numberingMapper.GetNumbering(p);
            if (numbering != null)
            {
                var cssRegistrator = context.Resolve<ICssRegistrator2>();
                var cssClass = cssRegistrator.RegisterParagraph(
                    p.ParagraphProperties,
                    (numbering.NumberingId, numbering.LevelIndex));


                context.AddNode(BuildContainerMax(cssClass.Name));
                context.AddNode(BuildContainerMin());
                context.AddNode(BuildNumberMax());
                context.AddNode(BuildNumberMin(p, cssRegistrator, numbering));
                context.AddMutation(BuildiInsertion(numbering.Verbose));
            }
        }

        private double PositionWithDelta(int delta = 0) => _config.NumberingIndex + _config.NumberingDelta * delta;

        private HtmlNode BuildContainerMax(string cssClass)
        {
            var node = new HtmlNode
            {
                Start = PositionWithDelta(0),
                End = PositionWithDelta(8),
                Tag = _config.NumberingContainerTag,
                Z = _config.NumberingContainerZ,
            };
            node.AddClasses(_config.NumberingContainerMaxCls);
            node.AddClasses(cssClass);
            return node;
        }

        private HtmlNode BuildContainerMin()
        {
            var node = new HtmlNode
            {
                Start = PositionWithDelta(1),
                End = PositionWithDelta(7),
                Tag = _config.NumberingContainerTag,
                Z = _config.NumberingContainerZ,
            };
            node.AddClasses(_config.NumberingContainerMinCls);
            return node;
        }

        private HtmlNode BuildNumberMax()
        {
            var node = new HtmlNode
            {
                Start = PositionWithDelta(2),
                End = PositionWithDelta(6),
                Tag = _config.NumberingContainerTag,
                Z = _config.NumberingContainerZ,
            };
            node.AddClasses(_config.NumberingNumberMaxCls);
            return node;
        }

        private HtmlNode BuildNumberMin(Paragraph p, ICssRegistrator2 cssRegistrator, IParagraphData paragraphData)
        {
            var node = new HtmlNode
            {
                Start = PositionWithDelta(3),
                End = PositionWithDelta(5),
                Tag = _config.NumberingNumberTag,
                Z = _config.NumberingNumberZ,
            };
            node.AddClasses(_config.NumberingNumberMinCls);

            var cssClass = cssRegistrator.RegisterRun(
                p.ParagraphProperties,
                p.ParagraphProperties?.ParagraphMarkRunProperties,
                (paragraphData.NumberingId, paragraphData.LevelIndex));
            node.AddClasses(cssClass.Name);

            var level = paragraphData.LevelXmlElement;
            if (level.LevelSuffix?.Val?.Value == LevelSuffixValues.Space)
            {
                node.SetStyle("padding-right", "0.5em");
            }
            else if (level.LevelSuffix?.Val?.Value == LevelSuffixValues.Nothing) { }
            else
            {
                node.SetStyle("padding-right", "1.5em");
            }

            return node;
        }

        private Mutation BuildiInsertion(string verbose) => new TextInsertion
        {
            Position = PositionWithDelta(4),
            Text = verbose
        };

        [PostProcessing]
        public void PostProcessing(IGlobalContext context)
        {
            var styleConfig = context.Resolve<StyleConfig>();
            var numConfig = context.Resolve<NumberingConfig>();
            context.AddCss(
                CSS(
                    styleConfig.LeftIdentationCssClassPrefix,
                    numConfig.NumberingContainerMinCls,
                    numConfig.NumberingNumberMinCls));
        }

        private static string CSS(
            string leftSpacerCls, 
            string numContainerMinCls, 
            string numNumberMinCls) =>
            $"{leftSpacerCls}, .{numContainerMinCls} {{ display: flex; }} " +
            $".{numNumberMinCls} {{ white-space: pre; }}";
    }
}
