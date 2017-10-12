using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Plugins.TextFixes
{
    /// <summary>
    /// Insert hypens from the open xml to the html document.
    /// </summary>
    public class HyphenInsertionPlugin
    {
        [ElementProcessing]
        public void InsertSoftHyphen(IElementContext context, SoftHyphen hyphen)
        {
            context.AddMutation(new TextInsertion
            {
                Position = context.TextIndex,
                Text = "&#173;"
            });
        }

        [ElementProcessing]
        public void InsertNoBreakHyphen(IElementContext context, NoBreakHyphen hyphen)
        {
            context.AddMutation(new TextInsertion
            {
                Position = context.TextIndex,
                Text = "&#8209;"
            });
        }
    }
}
