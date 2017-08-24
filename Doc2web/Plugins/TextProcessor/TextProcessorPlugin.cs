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
            context.AddNode(new HtmlNode
            {
                Start = context.TextIndex,
                End = context.Element.InnerText.Length,
                Tag = "p"
            });
            context.ProcessChilden();
        }

        [ElementProcessing]
        public void ProcessRun(IElementContext context, Run r)
        {
            context.AddNode(new HtmlNode
            {
                Start = context.TextIndex,
                End = context.TextIndex + r.InnerText.Length,
                Tag = "span"
            });
        }
    }
}
