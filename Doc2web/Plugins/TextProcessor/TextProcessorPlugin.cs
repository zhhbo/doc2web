using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.TextProcessor
{
    public class TextProcessorPlugin
    {
        public void ProcessParagraph(IElementContext context, Paragraph p)
        {
            context.AddNode(new HtmlNode
            {
                Start = 0,
                End = context.RootElementText.Length,
                Tag = "p"
            });
        }
    }
}
