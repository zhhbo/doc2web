using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Core.Rendering.Step1
{
    public static class FlatternHtmlNodes
    {

        /// <summary>
        /// Ensure that no html nodes are overlasping each other.
        /// </summary>
        /// <param name="nodes">Initial list of nodes created by the the processor.</param>
        /// <returns>Update list of nodes that are not overlapsing each other.</returns>
        public static List<HtmlNode> Flattern(List<HtmlNode> nodes)
        {
            List<HtmlNode> results = new List<HtmlNode>();
            List<List<HtmlNode>> layers = nodes.GroupByLayer();

            foreach (var layer in layers)
            {
                var flatLayer = FlatternLayer(layer);
                var combinedLayer = CombineFlatternedLayers(results, flatLayer);

                results.AddRange(combinedLayer);
            }

            results.SortByPriority();
            return results;
        }

        private static List<HtmlNode> FlatternLayer(List<HtmlNode> layer)
        {
            var singleLayerFlattern = new SingleLayerFlatterner();
            singleLayerFlattern.Nodes = layer;
            singleLayerFlattern.HandleIntersections();
            return singleLayerFlattern.Nodes;
        }

        private static List<HtmlNode> CombineFlatternedLayers(List<HtmlNode> upperLayer, List<HtmlNode> flatLayer)
        {
            var combineLayerFlattern = new CombineLayerFlatterner();
            combineLayerFlattern.UpperLayer = upperLayer;
            combineLayerFlattern.Nodes = flatLayer;
            combineLayerFlattern.HandleIntersections();
            return combineLayerFlattern.Nodes;
        }
    }
}
