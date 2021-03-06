﻿using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml;

namespace Doc2web.Core
{
    public class RootElementContext : INestableElementContext
    {
        private List<HtmlNode> _nodes;
        private List<Mutation> _mutations;
        private IGlobalContext _globalContext;

        public RootElementContext(IGlobalContext context, OpenXmlElement rootElement)
        {
            _globalContext = context;
            ViewBag = new Dictionary<string, object>();
            RootElement = rootElement;
            _nodes = new List<HtmlNode>();
            _mutations = new List<Mutation>();
        }


        public OpenXmlElement RootElement { get; private set; }

        public IDictionary<string, object> ViewBag { get; }

        public IEnumerable<HtmlNode> Nodes => _nodes;

        public IEnumerable<Mutation> Mutations => _mutations;

        public OpenXmlElement Element => RootElement;

        public int TextIndex => 0;

        public IContextNestingHandler NestingHandler { get; set; }

        public void AddNode(HtmlNode node) => _nodes.Add(node);

        public void AddNodes(IEnumerable<HtmlNode> nodes) => _nodes.AddRange(nodes);

        public void AddMutation(Mutation transformation) =>
            _mutations.Add(transformation);

        public void AddMutations(IEnumerable<Mutation> mutations) =>
            _mutations.AddRange(mutations);

        public void ProcessChilden()
        {
            int textIndex = 0;
            foreach (var elem in Element.ChildElements)
            {
                var childContext = new ChildElementContext(this)
                {
                    Element = elem,
                    TextIndex = textIndex
                };
                NestingHandler.QueueElementProcessing(childContext);
                textIndex += elem.InnerText.Length;
            }
        }

        public T Resolve<T>() => _globalContext.Resolve<T>();

        public bool TryResolve<T>(out T service) => _globalContext.TryResolve<T>(out service);
    }
}
