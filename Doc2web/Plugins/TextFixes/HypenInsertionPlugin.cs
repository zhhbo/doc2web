using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Plugins.TextFixes
{
    public class HyphenInsertionPlugin
    {
        [ElementProcessing]
        public void InsertSoftHyphen(IElementContext context, SoftHyphen hyphen)
        {
            context.AddMutation(new TextInsertion
            {
                Index = context.TextIndex,
                Text = "&#173;"
            });
        }

        [ElementProcessing]
        public void InsertNoBreakHyphen(IElementContext context, NoBreakHyphen hyphen)
        {
            context.AddMutation(new TextInsertion
            {
                Index = context.TextIndex,
                Text = "&#8209;"
            });
        }
    }
}
