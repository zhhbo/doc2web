using Autofac;
using Doc2web.Plugins.Style;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.TextProcessor
{
    public class TextProcessorPlugin
    {
        public string ContainerTag { get; set; }

        public string ContainerCls { get; set; }

        public int ContainerZ { get; set; }

        public string ParagraphTag { get; set; }

        public string ParagraphCls { get; set; }

        public int ParagraphZ { get; set; }

        public string RunTag { get; set; }

        public string RunCls { get; set; }

        public int RunZ { get; set; }

        public TextProcessorPlugin()
        {
            ContainerTag = "div";
            ContainerCls = "container";
            ContainerZ = 1_000_000;
            ParagraphTag = "p";
            ParagraphCls = "";
            ParagraphZ = 1_000;
            RunTag = "span";
            RunCls = "";
            RunZ = 1;
        }

        [ElementProcessing]
        public void ProcessParagraph(IElementContext context, Paragraph p)
        {
            var containerNode = new HtmlNode
            {
                Start = context.TextIndex,
                End = context.Element.InnerText.Length,
                Tag = ContainerTag,
                Z = ContainerZ,
            };
            containerNode.AddClass(ContainerCls);
            var pPr = p.ParagraphProperties;
            if (pPr != null)
            {
                var cssRegistrator = context.GlobalContext.Container.Resolve<ICssRegistrator>();
                var dynamicStyle = cssRegistrator.Register(pPr);
                if (dynamicStyle != "")
                    containerNode.AddClass(dynamicStyle);

                var styleId = p.ParagraphProperties?.ParagraphStyleId?.Val?.Value;
                if (styleId != null) {
                    containerNode.AddClass(cssRegistrator.Register(styleId));
                }
            }

            var pNode = new HtmlNode
            {
                Start = containerNode.Start,
                End = containerNode.End,
                Tag = ParagraphTag,
                Z = ParagraphZ,
            };
            pNode.AddClass(ParagraphCls);
            context.AddNode(containerNode);
            context.AddNode(pNode);

            context.ProcessChilden();
        }

        [ElementProcessing]
        public void ProcessRun(IElementContext context, Run r)
        {
            if (r.InnerText.Length == 0) return;
            var node = new HtmlNode
            {
                Start = context.TextIndex,
                End = context.TextIndex + r.InnerText.Length,
                Tag = RunTag,
                Z = RunZ,
            };
            node.AddClass(RunCls);

            var rPr = r.RunProperties;
            if (rPr != null)
            {
                var cssRegistrator = context.GlobalContext.Container.Resolve<ICssRegistrator>();
                var dynamicStyle = cssRegistrator.Register(rPr);
                if (dynamicStyle != "")
                    node.AddClass(dynamicStyle);

                var styleId = rPr.RunStyle?.Val?.Value;
                if (styleId != null)
                    node.AddClass(cssRegistrator.Register(styleId));
            }

            context.AddNode(node);
            context.ProcessChilden();
        }
    }
}
