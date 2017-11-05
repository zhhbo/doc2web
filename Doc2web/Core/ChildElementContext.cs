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

        public OpenXmlElement RootElement => _parent.RootElement;

        public OpenXmlElement Element { get; set; }

        public IDictionary<string, object> ViewBag => _parent.ViewBag;

        public int TextIndex { get; set; }

        public IEnumerable<HtmlNode> Nodes => _parent.Nodes;

        public IEnumerable<Mutation> Mutations => _parent.Mutations;

        public IContextNestingHandler NestingHandler { get => _parent.NestingHandler; set => _parent.NestingHandler = value; }

        public void AddNodes(IEnumerable<HtmlNode> nodes)
        {
            _parent.AddNodes(nodes);
        }

        public void AddMutations(IEnumerable<Mutation> mutations)
        {
            _parent.AddMutations(mutations);
        }

        public void AddNode(HtmlNode node)
        {
            _parent.AddNode(node);
        }

        public void AddMutation(Mutation mutation)
        {
            _parent.AddMutation(mutation);
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

        public T Resolve<T>() => _parent.Resolve<T>();

        public bool TryResolve<T>(out T service) => _parent.TryResolve(out service);

    }
}