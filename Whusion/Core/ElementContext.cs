using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml;

namespace Whusion.Core
{
    public class ElementContext : IElementContext
    {
        private List<HtmlNode> _nodes;
        private List<ITextTransformation> _transformations;

        public ElementContext(IGlobalContext context, OpenXmlElement rootElement)
        {
            GlobalContext = context;
            RootElement = rootElement;
            RootElementText = RootElement.InnerText;
            _nodes = new List<HtmlNode>();
            _transformations = new List<ITextTransformation>();
        }

        public IGlobalContext GlobalContext { get; private set; }

        public OpenXmlElement RootElement { get; private set; }

        public string RootElementText { get; set; }

        public IEnumerable<HtmlNode> Nodes => _nodes;

        public IEnumerable<ITextTransformation> Transformations => _transformations;

        public void AddNode(HtmlNode node) => _nodes.Add(node);

        public void AddMultipleNodes(IEnumerable<HtmlNode> nodes) => _nodes.AddRange(nodes);

        public void AddTranformation(ITextTransformation transformation) =>
            _transformations.Add(transformation);

        public void AddMultipleTransformations(IEnumerable<ITextTransformation> transformations) =>
            _transformations.AddRange(transformations);
    }
}
