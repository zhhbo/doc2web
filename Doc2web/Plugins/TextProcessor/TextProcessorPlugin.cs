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

        [ElementProcessing]
        public void ProcessParagraph(IElementContext context, Paragraph p)
        {
            var node = new HtmlNode
            {
                Start = context.TextIndex,
                End = context.Element.InnerText.Length,
                Tag = "p",
                Z = 100
            };
            var pPr = p.ParagraphProperties;
            if (pPr != null)
            {
                var cssRegistrator = context.GlobalContext.Container.Resolve<ICssRegistrator>();
                var dynamicStyle = cssRegistrator.Register(pPr);
                if (dynamicStyle != "")
                    node.AddClass(dynamicStyle);

                var styleId = p.ParagraphProperties?.ParagraphStyleId?.Val?.Value;
                if (styleId != null) {
                    node.AddClass(cssRegistrator.Register(styleId));
                }
            }


            context.AddNode(node);
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
                Tag = "span",
                Z = -100
            };

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
        }
    }
}
