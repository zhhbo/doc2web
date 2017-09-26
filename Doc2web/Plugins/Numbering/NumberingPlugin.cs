using Autofac;
using Doc2web.Plugins.Numbering.Mapping;
using Doc2web.Plugins.Style;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
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
                .Register(x => new NumberingMapper(_wpDoc))
                .As<INumberingMapper>()
                .SingleInstance();
        }

        [ElementProcessing]
        public void InsertNumbering(IElementContext context, Paragraph p)
        {
            var numberingMapper = context.Resolve<INumberingMapper>();

            var numbering = numberingMapper.GetNumbering(p);
            if (numbering != null)
            {
                var cssRegistrator = context.Resolve<ICssRegistrator>();
                var cssClass = cssRegistrator.RegisterNumbering(numbering.NumberingId, numbering.LevelIndex);

                context.AddNode(BuildContainerMax(cssClass));
                context.AddNode(BuildContainerMin());
                context.AddNode(BuildNumberMax());
                context.AddNode(BuildNumberMin(p, cssRegistrator, numbering.LevelXmlElement));
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
            node.AddClass(_config.NumberingContainerMaxCls);
            node.AddClass(cssClass);
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
            node.AddClass(_config.NumberingContainerMinCls);
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
            node.AddClass(_config.NumberingNumberMaxCls);
            return node;
        }

        private HtmlNode BuildNumberMin(Paragraph p, ICssRegistrator icssRegistrator, Level level)
        {
            var node = new HtmlNode
            {
                Start = PositionWithDelta(3),
                End = PositionWithDelta(5),
                Tag = _config.NumberingNumberTag,
                Z = _config.NumberingNumberZ,
            };
            node.AddClass(_config.NumberingNumberMinCls);

            var dynProp = p.ParagraphProperties?.ParagraphMarkRunProperties;
            if (dynProp != null)
            {
                var cls = icssRegistrator.RegisterRunProperties(dynProp);
                node.AddClass(cls);
            }

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
