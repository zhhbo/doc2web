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

        public IEnumerable<Mutation> Mutations => _parent.Mutations;

        public IContextNestingHandler NestingHandler { get => _parent.NestingHandler; set => _parent.NestingHandler = value; }

        public void AddMultipleNodes(IEnumerable<HtmlNode> nodes)
        {
            _parent.AddMultipleNodes(nodes);
        }

        public void AddMutations(IEnumerable<Mutation> transformations)
        {
            _parent.AddMutations(transformations);
        }

        public void AddNode(HtmlNode node)
        {
            _parent.AddNode(node);
        }

        public void AddMutation(Mutation transformation)
        {
            _parent.AddMutation(transformation);
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