using Autofac;
using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Css;
using Doc2web.Plugins.Style.Properties;
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
                var cssRegistrator = context.Resolve<ICssRegistrator2>();
                var cssClass = cssRegistrator.RegisterParagraph(pPr);
                containerNode.AddClasses(cssClass.Name);
                AddIndentationIfRequired(context, cssClass);
            }
        }

        private void AddIndentationIfRequired(IElementContext context, CssClass2 cssClass)
        {
            if (cssClass.Name.Length == 0) return;

            var identation = cssClass?.Props.Get<IdentationCssProperty>();
            if (identation == null) return;

            if (identation.LeftIndent.HasValue)
                context.AddNode(BuildLeftIdentation());
            if (identation.RightIndent.HasValue)
                context.AddNode(BuildRightIdentation());
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
            var cssRegistrator = context.Resolve<ICssRegistrator2>();
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

        public void PostProcess(IGlobalContext context)
        {
            context.AddCss(RequiredCss);
        }

        private string RequiredCss =>
            $"{_config.ContainerTag}.{_config.ContainerCls} {{display: flex; flex-direction: column}}";
    }
}
