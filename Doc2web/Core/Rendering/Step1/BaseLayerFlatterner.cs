using Doc2web.Core.Rendering.Step1;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Core.Rendering.Step1
{
    internal abstract class BaseLayerFlatterner
    {
        public List<HtmlNode> Nodes { get; set; }

        public void HandleIntersections()
        {
            List<HtmlNode> result = new List<HtmlNode>();
            Nodes.SortByPriority();

            for (var nIndex = 0; nIndex < Nodes.Count; nIndex++)
                nIndex = AddIntersectionNodes(nIndex);

            Nodes.SortByPriority();
        }

        private int AddIntersectionNodes(int nIndex)
        {
            var node = Nodes[nIndex];
            int[] intersections = FindIntersections(node);

            foreach (var i in intersections)
            {
                node = InsertIntersection(nIndex, node, i);
            }

            nIndex += intersections.Length;
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
}
