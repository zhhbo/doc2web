using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Core.Rendering.Step1
{
    internal static class Extensions
    {
        public static void SortByPriority(this List<HtmlNode> nodes) => nodes.Sort(Compare);

        private static int Compare(HtmlNode a, HtmlNode b)
        {
            if (a.Z > b.Z) return -1;
            if (b.Z > a.Z) return 1;
            if (a.Start < b.Start) return -1;
            if (b.Start < a.Start) return 1;
            if (a.End > b.End) return -1;
            if (b.End > a.End) return 1;
            return 0;
        }

        
        public static List<List<HtmlNode>> GroupByLayer(this List<HtmlNode> nodes) =>
             nodes
            .GroupBy(x => x.Z)
            .OrderByDescending(x => x.Key)
            .Select(x => x.ToList())
            .ToList();
    }
}
