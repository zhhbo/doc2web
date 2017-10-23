using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Doc2web.Plugins.TableOfContent
{
    public class TableOfContentPlugin
    {
        private static Regex tocRegex = new Regex(@"PAGEREF _Toc\d+(\s\\[a-z])+\s", RegexOptions.Compiled);
        private TableOfContentConfig _config;

        public TableOfContentPlugin() : this(new TableOfContentConfig()) { }

        public TableOfContentPlugin(TableOfContentConfig config)
        {
            _config = config;
        }

        [ElementProcessing]
        public void ProcessHyperlink(IElementContext context, Hyperlink a)
        {
            if (context.RootElement.LastChild == a)
            {
                var m = tocRegex.Match(context.RootElement.InnerText);
                if (!m.Success) return;

                ApplyTOC(context, a, m);
            }

        }

        private void ApplyTOC(IElementContext context, Hyperlink a, Match m)
        {
            RemovePreviousText(context);
            ReplaceCrossRefBySpacer(context, m);
            AddCssClassParagraph(context.Nodes);
            context.ProcessChilden();
        }

        private void AddCssClassParagraph(IEnumerable<HtmlNode> nodes)
        {
            var container = nodes
                .FirstOrDefault(x => x.Start == _config.ParagraphStart && x.Tag == _config.ParagraphTag);
            container?.AddClasses(_config.ParagraphTocCssClass);
        }

        private void ReplaceCrossRefBySpacer(IElementContext context, Match m)
        {
            context.AddMutation(new TextDeletion
            {
                Position = m.Index,
                Count = m.Length,
            });
            context.AddNode(GenerateSpaceNode(m.Index));
        }

        private HtmlNode GenerateSpaceNode(int index)
        {
            var node = new HtmlNode
            {
                Start = index,
                End = index,
                Tag = _config.SpacerTag,
                Z = _config.SpacerZ
            };
            node.AddClasses(_config.SpacerCssClass);
            return node;
        }

        private void RemovePreviousText(IElementContext context)
        {
            context.AddMutation(new TextDeletion{
                Position = 0,
                Count = context.TextIndex
            });
        }

        [PostProcessing]
        public void PostProcessing(IGlobalContext context)
        {
            context.AddCss(CSS(_config.ParagraphTocCssClass, _config.SpacerCssClass));
        }

        public static string CSS(string pCssClass, string spacerCssClass) =>
            $".{pCssClass} " + "{ display: flex; flex-direction: row; }" + 
            $".{spacerCssClass}" + "{ flex: 1; border-bottom:1px dotted black; " +
            "margin: 0 5pt; position: relative; top: -3pt;}";
    }
}
