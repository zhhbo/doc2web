using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Plugins.TextFixes
{
    public class EscapeHtmlPlugin
    {
        [ElementProcessing]
        public void EscapeCharacters(IElementContext context, Paragraph p)
        {
            string text = p.InnerText;
            var mutations = FindEscapeChar(context.TextIndex, text);
            if (mutations.Any()) context.AddMutations(mutations);
        }

        private IEnumerable<Mutation> FindEscapeChar(int offset, string text)
        {
            for(int i = 0; i < text.Length; i++)
            {
                if (text[i] == '&')
                     yield return BuildReplacement(i + offset, "&amp;");
                if (text[i] == '>')
                     yield return BuildReplacement(i + offset, "&gt;");
                if (text[i] == '<')
                     yield return BuildReplacement(i + offset, "&lt;");
            }
        }

        private Mutation BuildReplacement(int i, string replacement) =>
            new TextReplacement
            {
                Position = i,
                Length = 1,
                Replacement = replacement
            };

    }
}
