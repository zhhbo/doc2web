using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Plugins.TextFixes
{
    public class BreakInsertionPlugin
    {
        public void BrInsertion(IElementContext context, Break br)
        {
            InsertBrAtCurrentPosition(context);
        }

        public void CrInsertion(IElementContext context, CarriageReturn cr)
        {
            InsertBrAtCurrentPosition(context);
        }

        private void InsertBrAtCurrentPosition(IElementContext context)
        {
            context.AddNode(new HtmlNode
            {
                Start = context.TextIndex,
                Tag = "br"
            });
        }
    }
}
