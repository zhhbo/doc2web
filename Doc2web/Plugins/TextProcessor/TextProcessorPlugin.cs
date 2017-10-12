using Autofac;
using Doc2web.Plugins.Numbering;
using Doc2web.Plugins.Numbering.Mapping;
using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Css;
using Doc2web.Plugins.Style.Properties;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Plugins.TextProcessor
{
    public class TextProcessorPlugin
    {
        private TextProcessorConfig _config;

        public TextProcessorPlugin() : this(new TextProcessorConfig())
        {
        }

        public TextProcessorPlugin(TextProcessorConfig config)
        {
            _config = config;
        }

        [ElementProcessing]
        public void ProcessParagraph(IElementContext context, Paragraph p)
        {
            if (p != context.RootElement) return;
            var containerNode = BuildContainer();
            ProcessParagraphProperties(context, p, containerNode);

            context.AddNode(containerNode);
            context.AddNode(BuildPNode(context, p, containerNode));

            context.ProcessChilden();
        }

        private HtmlNode BuildContainer()
        {
            var containerNode = new HtmlNode
            {
                Start = _config.ContainerStart,
                End = _config.ContainerEnd,
                Tag = _config.ContainerTag,
                Z = _config.ContainerZ,
            };
            containerNode.AddClasses(_config.ContainerCls);
            return containerNode;
        }

        private void ProcessParagraphProperties(IElementContext context, Paragraph p, HtmlNode containerNode)
        {
            var pPr = p.ParagraphProperties;
            if (pPr != null)
            {
                var cssRegistrator = context.Resolve<ICssRegistrator>();
                var numberingConfig = context.Resolve<NumberingConfig>();
                var styleConfig = context.Resolve<StyleConfig>();

                CssClass cssClass;
                if (context.ViewBag.TryGetValue(numberingConfig.ParagraphNumberingDataKey, out object numbering))
                {
                    var numberingData = ((int, int))numbering;
                    containerNode.AddClasses(_config.ContainerWithNumberingCls);
                    cssClass = cssRegistrator.RegisterParagraph(pPr, numberingData);
                } else
                {
                    cssClass = cssRegistrator.RegisterParagraph(pPr);
                }

                containerNode.AddClasses(cssClass.Name);
                context.ViewBag[_config.PPropsCssClassKey] = cssClass;
            }
        }

        private HtmlNode BuildPNode(IElementContext context, Paragraph p, HtmlNode containerNode)
        {
            var pNode = new HtmlNode
            {
                Start = context.TextIndex - _config.Delta,
                End = context.TextIndex + p.InnerText.Length + _config.Delta,
                Tag = _config.ParagraphTag,
                Z = _config.ParagraphZ,
            };
            pNode.AddClasses(_config.ParagraphCls);
            if (p.InnerText.Length == 0) pNode.TextPrefix = "&#8203;";
            return pNode;
        }

        private HtmlNode BuildLeftIdentation()
        {
            var node = new HtmlNode
            {
                Start = _config.ContainerStart + _config.Delta,
                End = _config.ContainerStart + _config.Delta * 2,
                Tag = _config.IdentationTag,
                Z = _config.ParagraphZ
            };
            node.AddClasses(_config.LeftIdentationCls);
            return node;
        }

        private HtmlNode BuildRightIdentation()
        {
            var node = new HtmlNode
            {
                Start = _config.ContainerEnd - _config.Delta * 2,
                End = _config.ContainerEnd - _config.Delta,
                Tag = _config.IdentationTag,
                Z = _config.ParagraphZ
            };
            node.AddClasses(_config.RightIndentationCls);
            return node;
        }

        [ElementProcessing]
        public void ProcessRun(IElementContext context, Run r)
        {
            var cssRegistrator = context.Resolve<ICssRegistrator>();
            if (r.InnerText.Length > 0)
            {
                var node = new HtmlNode
                {
                    Start = context.TextIndex + _config.Delta,
                    End = context.TextIndex + r.InnerText.Length,
                    Tag = _config.RunTag,
                    Z = _config.RunZ,
                };
                node.AddClasses(_config.RunCls);

                var pPr = (context.RootElement as Paragraph)?.ParagraphProperties;
                var rPr = r.RunProperties;
                var cls = cssRegistrator.RegisterRun(pPr, rPr, null);
                node.AddClasses(cls.Name);

                context.AddNode(node);

            }
            context.ProcessChilden();
        }

        [PostProcessing]
        public void PostProcess(IGlobalContext context)
        {
            var marginApplier = new MarginApplier(_config, context.RootElements.ToArray());
            marginApplier.Apply();
        }
    }
}
