using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Core.Rendering.Step1
{
    public class FlatternPrototype
    {
        private class FirstComparer : IComparer<HtmlNode>
        {
            public int Compare(HtmlNode x, HtmlNode y)
            {
                if (x.Z > y.Z) return -1;
                if (y.Z > x.Z) return 1;
                if (x.Start > y.Start) return -1;
                if (y.Start > x.Start) return 1;
                return 0;
            }
        }

        public static void Flattern(List<HtmlNode> nodes)
        {
            nodes.Sort(new FirstComparer());

            int i = 1;

            while (i < nodes.Count)
            {
                var n = nodes[i];
                int j = 0;
                while (j < i && nodes[j].Z > n.Z && nodes[j].Start < n.End) 
                {
                    if (nodes[j].Start > n.Start && nodes[j].Start < n.End)
                    {
                        var colisiton = nodes[j].Start;
                        HandleColision(nodes, i, colisiton);
                        break;
                    }
                    if (nodes[j].End > n.Start && nodes[j].End < n.End)
                    {
                        var colisiton = nodes[j].End;
                        HandleColision(nodes, i, colisiton);
                        break;
                    }

                    j++;
                }
                i++;
            }
        }

        private static void HandleColision(List<HtmlNode> nodes, int index, int colisiton)
        {
            var n = nodes[index];
            var lastEnd = n.End;
            var newNode = n.Clone();
            n.End = colisiton;
            newNode.Start = colisiton;
            nodes.Insert(index + 1, newNode);
        }
    }
}
