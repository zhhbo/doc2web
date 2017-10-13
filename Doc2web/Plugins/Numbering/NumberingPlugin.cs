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
    /// <summary>
    /// Add spacers and automated numbering in the margin.
    /// </summary>
    public class NumberingPlugin
    {
        private WordprocessingDocument _wpDoc;
        private NumberingConfig _config;
        private INumberingMapper _numberingMapper;

        public NumberingPlugin(WordprocessingDocument wpDoc) : this(wpDoc, new NumberingConfig()) { }

        public NumberingPlugin(WordprocessingDocument wpDoc, NumberingConfig config)
        {
            _wpDoc = wpDoc;
            _config = config;
        }

        public NumberingPlugin(INumberingMapper numberingMapper) : this(numberingMapper, new NumberingConfig()) { }

        public NumberingPlugin(INumberingMapper numberingMapper, NumberingConfig config)
        {
            _numberingMapper = numberingMapper;
            _config = config;
        }


        [InitializeEngine]
        public void InitEngine(ContainerBuilder builder)
        {
            builder.RegisterInstance(_config);

            if (_wpDoc != null) RegisterFromWpDoc(builder);
            else RegisterFromNumberingMapper(builder);
        }

        private void RegisterFromNumberingMapper(ContainerBuilder builder)
        {
            builder
                .RegisterInstance(_numberingMapper)
                .ExternallyOwned();
        }

        private void RegisterFromWpDoc(ContainerBuilder builder)
        {
            builder
                .RegisterInstance(_wpDoc)
                .ExternallyOwned();
            builder
                .RegisterType<NumberingMapper>()
                .As<INumberingMapper>()
                .SingleInstance();
        }

        [ElementProcessing]
        public void InsertNumbering(IElementContext context, Paragraph p)
        {
            var numberingMapper = context.Resolve<INumberingMapper>();

            if (!numberingMapper.IsValid) return;

            var paragraphData = numberingMapper.GetNumbering(p);
            if (paragraphData != null)
            {
                context.ViewBag[_config.ParagraphNumberingDataKey] = 
                    (paragraphData.NumberingId, paragraphData.LevelIndex);
                var cssRegistrator = context.Resolve<ICssRegistrator>();
                context.AddNode(BuildNumberingContainer());
                context.AddNode(BuildNumberingNumber(p, cssRegistrator, paragraphData));
            }
        }

        private double PositionWithDelta(int delta = 0) => 
            _config.NumberingStartingPosition + _config.NumberingDelta * delta;

        private HtmlNode BuildNumberingContainer()
        {
            var node = new HtmlNode
            {
                Start = PositionWithDelta(0),
                End = PositionWithDelta(3),
                Tag = _config.NumberingContainerTag,
                Z = _config.NumberingContainerZ,
            };
            node.AddClasses(_config.NumberingContainerCls);
            return node;
        }

        private HtmlNode BuildNumberingNumber(
            Paragraph p, 
            ICssRegistrator cssRegistrator, 
            IParagraphData paragraphData)
        {
            var node = new HtmlNode
            {
                Start = PositionWithDelta(1),
                End = PositionWithDelta(2),
                Tag = _config.NumberingNumberTag,
                Z = _config.NumberingNumberZ,
                TextPrefix = paragraphData.Verbose
            };
            node.AddClasses(_config.NumberingNumberCls);

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
            else node.SetStyle("padding-right", "1.5em");

            return node;
        }

        [PostProcessing]
        public void PostProcessing(IGlobalContext context)
        {
            var numConfig = context.Resolve<NumberingConfig>();
            context.AddCss(
                CSS(numConfig.NumberingContainerCls,
                    numConfig.NumberingNumberCls));
        }

        private static string CSS(
            string numberingContainerCls, 
            string numberingNumberCls) =>
            $".{numberingContainerCls} {{ display: block; text-align: right; }} " +
            $".{numberingNumberCls} {{ display: inline-block; white-space: pre; }}";
    }
}
