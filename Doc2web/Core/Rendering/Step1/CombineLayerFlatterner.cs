using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Core.Rendering.Step1
{
    internal class CombineLayerFlatterner : BaseLayerFlatterner
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

}
