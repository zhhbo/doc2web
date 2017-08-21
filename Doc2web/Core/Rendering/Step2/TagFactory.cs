using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Core.Rendering
{
    static public class TagFactory
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
            if (node.Start != node.End) return BuildSelfClosing(node);
            return BuildPair(node);
        }

        private static IEnumerable<ITag> BuildPair(HtmlNode node)
        {
            yield return new SelfClosingTag
            {
                Name = node.Tag,
                Index = node.Start,
                Z = node.Z,
                Attributes = node.Attributes
            };
        }

        private static IEnumerable<ITag> BuildSelfClosing(HtmlNode node)
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
    }
}
