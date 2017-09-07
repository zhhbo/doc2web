using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Core.Rendering.Step2
{
    static public class TagsFactory
    {
        public static ITag[] Build(List<HtmlNode> nodes)
        {
            var tags = BuildTags(nodes).ToArray();
            Array.Sort(tags, new TagComparer());
            return tags;
        }

        private static IEnumerable<ITag> BuildTags(List<HtmlNode> nodes) =>
            nodes
            .SelectMany(BuildTagsFromNode)
            .ToArray();

        private static IEnumerable<ITag> BuildTagsFromNode(HtmlNode node)
        {
            if (_selfClosingTags.Contains(node.Tag)) return BuildSelfClosing(node);
            return BuildPair(node);
        }

        private static SortedSet<string> _selfClosingTags = new SortedSet<string>
        {
            "area",
            "base",
            "br",
            "col",
            "command",
            "embed",
            "hr",
            "img",
            "input",
            "keygen",
            "link",
            "meta",
            "param",
            "source",
            "track",
            "wbr",
        };

        private static IEnumerable<ITag> BuildPair(HtmlNode node)
        {
            var opening = new OpeningTag
            {
                Name = node.Tag,
                Index = node.Start,
                Z = node.Z,
                Attributes = node.Attributes
            };
            var closing = new ClosingTag
            {
                Related = opening,
                Index = node.End
            };
            opening.Related = closing;
            yield return opening;
            yield return closing;
        }

        private static IEnumerable<ITag> BuildSelfClosing(HtmlNode node)
        {
            yield return new SelfClosingTag
            {
                Name = node.Tag,
                Index = node.Start,
                Z = node.Z,
                Attributes = node.Attributes
            };
        }
    }
}
