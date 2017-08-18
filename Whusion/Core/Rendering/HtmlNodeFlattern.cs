using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Whusion.Core.Rendering
{
    public static class HtmlNodeFlattern
    {
        private class SingleLayerFlattern
        {
            private int _i = 0;

            public List<HtmlNode> Nodes { get; set; }

            public void HandleIntersections()
            {
                List<HtmlNode> result = new List<HtmlNode>();
                Nodes.Sort(SortNodesByStartEnd);

                for(var nIndex = 0; nIndex < Nodes.Count; nIndex++)
                    nIndex = AddIntersectionNodes(nIndex);

                Nodes.Sort(SortNodesByStartEnd);
            }

            private int AddIntersectionNodes(int nIndex)
            {
                var node = Nodes[nIndex];
                int[] intersections = FindIntersections(node);

                foreach (var i in intersections)
                {
                    node = InsertIntersection(nIndex, node, i);
                }

                nIndex += intersections.Count();
                return nIndex;
            }

            private HtmlNode InsertIntersection(int nIndex, HtmlNode node, int i)
            {
                var newNode = node.Clone();
                newNode.Start = i;
                newNode.End = node.End;
                node.End = i;
                Nodes.Insert(nIndex, newNode);
                node = newNode;
                return node;
            }

            private int[] FindIntersections(HtmlNode node)
            {
                return Nodes
                    .SelectMany(n => new int[] { n.Start, n.End })
                    .Where(i => node.Start < i && i < node.End)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToArray();
            }
        }

        public static List<HtmlNode> Flattern(List<HtmlNode> nodes)
        {
            // Sort in order;
            var singleLayerFlattern = new SingleLayerFlattern();
            singleLayerFlattern.Nodes = nodes;
            singleLayerFlattern.HandleIntersections();

            return nodes;
        }

        private static int SortNodesByStartEnd(HtmlNode a, HtmlNode b)
        {
            if (a.Start < b.Start) return -1;
            if (b.Start < a.Start) return 1;
            if (a.End > b.End) return -1;
            if (b.End > a.End) return 1;
            return 0;
        }
    }
}
