﻿using Autofac;
using Doc2web.Plugins.Style;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
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
            var containerNode = new HtmlNode
            {
                Start = _config.ContainerStart,
                End = _config.ContainerEnd,
                Tag = _config.ContainerTag,
                Z = _config.ContainerZ,
            };
            containerNode.AddClass(_config.ContainerCls);
            context.AddNode(containerNode);
            context.AddNode(BuildLeftIdentation());
            context.AddNode(BuildPNode(context, p, containerNode));
            context.AddNode(BuildRightIdentation());

            context.ProcessChilden();
        }

        private HtmlNode BuildPNode(IElementContext context, Paragraph p, HtmlNode containerNode)
        {
            var pPr = p.ParagraphProperties;
            if (pPr != null)
            {
                var cssRegistrator = context.GlobalContext.Container.Resolve<ICssRegistrator>();
                var dynamicStyle = cssRegistrator.RegisterParagraphProperties(pPr);
                if (dynamicStyle != "")
                    containerNode.AddClass(dynamicStyle);

                var styleId = p.ParagraphProperties?.ParagraphStyleId?.Val?.Value;
                if (styleId != null)
                {
                    containerNode.AddClass(cssRegistrator.RegisterStyle(styleId));
                }
            }

            var pNode = new HtmlNode
            {
                Start = context.TextIndex - _config.Delta,
                End = context.TextIndex + p.InnerText.Length + _config.Delta,
                Tag = _config.ParagraphTag,
                Z = _config.ParagraphZ,
            };
            pNode.AddClass(_config.ParagraphCls);
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
            node.AddClass(_config.LeftIdentationCls);
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
            node.AddClass(_config.RightIndentationCls);
            return node;
        }

        [ElementProcessing]
        public void ProcessRun(IElementContext context, Run r)
        {
            if (r.InnerText.Length > 0)
            {
                var node = new HtmlNode
                {
                    Start = context.TextIndex + _config.Delta,
                    End = context.TextIndex + r.InnerText.Length,
                    Tag = _config.RunTag,
                    Z = _config.RunZ,
                };
                node.AddClass(_config.RunCls);

                var rPr = r.RunProperties;
                if (rPr != null)
                {
                    var cssRegistrator = context.GlobalContext.Container.Resolve<ICssRegistrator>();
                    var dynamicStyle = cssRegistrator.RegisterRunProperties(rPr);
                    if (dynamicStyle != "")
                        node.AddClass(dynamicStyle);

                    var styleId = rPr.RunStyle?.Val?.Value;
                    if (styleId != null)
                        node.AddClass(cssRegistrator.RegisterStyle(styleId));
                }

                context.AddNode(node);

            }
            context.ProcessChilden();
        }

        public void PostProcess(IGlobalContext context)
        {
            context.AddCss(RequiredCss);
        }

        private string RequiredCss =>
            $"{_config.ContainerTag}.{_config.ContainerCls} {{display: flex; flex-direction: column}}";
    }
}
