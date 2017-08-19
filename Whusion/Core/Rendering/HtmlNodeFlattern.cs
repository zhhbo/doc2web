using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Whusion.Core.Rendering
{
    public static class HtmlNodeFlattern
    {
        abstract class IntersectionsHandler
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

            protected abstract int[] FindIntersections(HtmlNode node);
        }

        class SingleLayerFlattern : IntersectionsHandler
        {
            protected override int[] FindIntersections(HtmlNode node) =>
                Nodes
                .SelectMany(n => new int[] { n.Start, n.End })
                .Where(i => node.Start < i && i < node.End)
                .Distinct()
                .OrderBy(x => x)
                .ToArray();
        }

        class CombineLayerFlattern : IntersectionsHandler
        {
            private List<HtmlNode> _upperLayer;
            private int[] _potentialIntersections;

            public List<HtmlNode> UpperLayer
            {
                get => _upperLayer;
                set
                {
                    _upperLayer = value;
                    _potentialIntersections =
                        _upperLayer
                        .SelectMany(n => new int[] { n.Start, n.End })
                        .Distinct()
                        .OrderBy(x => x)
                        .ToArray();
                }
            }

            protected override int[] FindIntersections(HtmlNode node) =>
                _potentialIntersections
                .Where(i => node.Start < i && i < node.End)
                .ToArray();
        }

        public static List<HtmlNode> Flattern(List<HtmlNode> nodes)
        {
            List<HtmlNode> results = new List<HtmlNode>();
            List<List<HtmlNode>> layers = GroupByLayer(nodes);

            foreach (var layer in layers)
            {
                var flatLayer = FlatternLayer(layer);
                var combinedLayer = CombineFlatternedLayer(results, flatLayer);

                results.AddRange(combinedLayer);
            }

            results.Sort(SortNodesByStartEnd);
            return results;
        }

        private static List<List<HtmlNode>> GroupByLayer(List<HtmlNode> nodes)
        {
            return nodes
                .GroupBy(x => x.Z)
                .OrderByDescending(x => x.Key)
                .Select(x => x.ToList())
                .ToList();
        }

        private static List<HtmlNode> FlatternLayer(List<HtmlNode> layer)
        {
            var singleLayerFlattern = new SingleLayerFlattern();
            singleLayerFlattern.Nodes = layer;
            singleLayerFlattern.HandleIntersections();
            return singleLayerFlattern.Nodes;
        }

        private static List<HtmlNode> CombineFlatternedLayer(List<HtmlNode> upperLayer, List<HtmlNode> flatLayer)
        {
            var combineLayerFlattern = new CombineLayerFlattern();
            combineLayerFlattern.UpperLayer = upperLayer;
            combineLayerFlattern.Nodes = flatLayer;
            combineLayerFlattern.HandleIntersections();
            return combineLayerFlattern.Nodes;
        }

        static int SortNodesByStartEnd(HtmlNode a, HtmlNode b)
        {
            if (a.Z > b.Z) return -1;
            if (b.Z > a.Z) return 1;
            if (a.Start < b.Start) return -1;
            if (b.Start < a.Start) return 1;
            if (a.End > b.End) return -1;
            if (b.End > a.End) return 1;
            return 0;
        }
    }
}
