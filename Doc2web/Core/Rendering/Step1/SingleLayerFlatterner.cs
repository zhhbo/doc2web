using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Core.Rendering.Step1
{
    internal class SingleLayerFlatterner : BaseLayerFlatterner
    {
        protected override int[] FindIntersections(HtmlNode node) =>
            Nodes
            .SelectMany(n => new int[] { n.Start, n.End })
            .Where(i => node.Start < i && i < node.End)
            .Distinct()
            .OrderBy(x => x)
            .ToArray();
    }
}
