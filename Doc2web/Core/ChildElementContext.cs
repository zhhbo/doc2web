using System.Collections.Generic;
using DocumentFormat.OpenXml;

namespace Doc2web.Core
{
    public class ChildElementContext : INestableElementContext
    {
        private INestableElementContext _parent;

        public ChildElementContext(INestableElementContext rootElementContext)
        {
            _parent = rootElementContext;
        }

        public IGlobalContext GlobalContext => _parent.GlobalContext;

        public OpenXmlElement RootElement => _parent.RootElement;

        public OpenXmlElement Element { get; set; }

        public int TextIndex { get; set; }

        public IEnumerable<HtmlNode> Nodes => _parent.Nodes;

        public IEnumerable<ITextTransformation> Transformations => _parent.Transformations;

        public IContextNestingHandler NestingHandler { get => _parent.NestingHandler; set => _parent.NestingHandler = value; }

        public void AddMultipleNodes(IEnumerable<HtmlNode> nodes)
        {
            _parent.AddMultipleNodes(nodes);
        }

        public void AddMultipleTransformations(IEnumerable<ITextTransformation> transformations)
        {
            _parent.AddMultipleTransformations(transformations);
        }

        public void AddNode(HtmlNode node)
        {
            _parent.AddNode(node);
        }

        public void AddTranformation(ITextTransformation transformation)
        {
            _parent.AddTranformation(transformation);
        }

        public void ProcessChilden()
        {
            int textIndex = TextIndex;
            foreach (var elem in Element.ChildElements)
            {
                var childContext = new ChildElementContext(this)
                {
                    Element = elem,
                    TextIndex = textIndex
                };
                _parent.NestingHandler.QueueElementProcessing(childContext);
                textIndex += elem.InnerText.Length;
            }
        }

    }
}