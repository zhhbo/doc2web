using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Core.Rendering
{
    public static class TagsSorter
    {
        public static ITag[] Sort(ITag[] tags) =>
            tags
            .GroupBy(x => x.Position)
            .SelectMany(SortCluster)
            .OrderBy(x => x.Position)
            .ToArray();

        private static IEnumerable<ITag> SortCluster(IEnumerable<ITag> cluster)
        {
            var sortedCluster = cluster
                .GroupBy(ClassifySubCluster)
                .OrderBy(x => x.Key)
                .SelectMany(SortSubCluster)
                .ToArray();

            for (int i = 0; i < sortedCluster.Length; i++)
                sortedCluster[i].Position = sortedCluster[i].Position + double.Epsilon * i;

            return sortedCluster;
        }

        private static int ClassifySubCluster(ITag x)
        {
            var delta = x.RelatedPosition - x.Position;
            if (delta > 0) return 1;
            if (delta < 0) return -1;
            return 0;
        }

        private static IEnumerable<ITag> SortSubCluster(IGrouping<int, ITag> arg)
        {
            if (arg.Key < 0) return arg.OrderBy(x => -1 * (x.RelatedPosition - x.Position));
            if (arg.Key > 0) return arg.OrderBy(x => -1 * (x.RelatedPosition - x.Position));
            return arg.OrderBy(x => (x.Name, GetNumForType(x)));
        }

        private static int GetNumForType(ITag x)
        {
            if (x is OpeningTag) return -1;
            if (x is ClosingTag) return 1;
            return 0;
        }

    }
}
