using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Doc2web.Plugins.CrossReferences
{
    public class CrossReferencesCleanupPlugin
    {
        private static Regex crossReferenceRegex =
            new Regex(
                @"(REF\s)?_?Ref\d+(\s\\{1,2}[a-z])*(\s+\\{1,2}\*\sMERGEFORMAT\s*)?",
                RegexOptions.Compiled);


        [ElementProcessing]
        public void RemoveCrossRef(IElementContext context, Paragraph p)
        {
            foreach(Match match in crossReferenceRegex.Matches(p.InnerText))
            {
                context.AddMutation(new TextDeletion
                {
                    Index = match.Index,
                    Length = match.Length
                });
            }
        }
    }
}
