﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Core.Rendering
{
    public static class HtmlNodesFlatterner
    {

        private class FirstComparer : IComparer<HtmlNode>
        {
            public int Compare(HtmlNode x, HtmlNode y)
            {
                if (x.Z > y.Z) return -1;
                if (y.Z > x.Z) return 1;
                if (x.Start < y.Start) return -1;
                if (y.Start < x.Start) return 1;
                return 0;
            }
        }

        public static void Flattern(List<HtmlNode> nodes)
        {
            nodes.Sort(new FirstComparer());

            int i = 1;

            while (i < nodes.Count)
            {
                var target = nodes[i];
                int j = 0;
                while (j < i && nodes[j].Z > target.Z)
                {
                    var other = nodes[j];
                    if (other.Start > target.Start && other.Start < target.End)
                    {
                        nodes.Insert(i + 1, SplitNode(target, other.Start));
                        break;
                    }
                    if (other.End > target.Start && other.End < target.End)
                    {
                        nodes.Insert(i + 1, SplitNode(target, other.End));
                        break;
                    }

                    j++;
                }
                i++;
            }
        }

        private static HtmlNode SplitNode(HtmlNode n, double colisiton)
        {
            var lastEnd = n.End;
            var newNode = n.Clone();
            n.End = colisiton;
            newNode.Start = colisiton;
            return newNode;
        }
    }
}
