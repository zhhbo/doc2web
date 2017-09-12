using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Core.Rendering.Step2
{
    static public class TagsFactory
    {
        public static ITag[] Build(HtmlNode[] nodes)
        {
            var tags = BuildTags(nodes).ToArray();
            Array.Sort(tags, new TagComparer());
            return tags;
        }

        private static ITag[] BuildTags(HtmlNode[] nodes)
        {
            var tags = new ITag[nodes.Length * 2];
            int j = 0;

            for (int i = 0; i < nodes.Length; i++)
                j = AddTagsToArray(nodes[i], tags, j);

            if (tags.Length != j) Array.Resize(ref tags, j);
            return tags;
        }

        private static int AddTagsToArray(HtmlNode node, ITag[] tags, int j)
        {
            if (_selfClosingTags.Contains(node.Tag))
            {
                tags[j] = BuildSelfClosing(node);
                j++;
            }
            else
            {
                var nTags = BuildPair(node);
                tags[j] = nTags[0];
                tags[j + 1] = nTags[1];
                j += 2;
            }
            return j;
        }

        private static HashSet<string> _selfClosingTags = new HashSet<string>
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

        private static ITag[] BuildPair(HtmlNode node)
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
            return new ITag[2] { opening, closing };
        }

        private static ITag BuildSelfClosing(HtmlNode node)
        {
            return new SelfClosingTag
            {
                Name = node.Tag,
                Index = node.Start,
                Z = node.Z,
                Attributes = node.Attributes
            };
        }
    }
}
