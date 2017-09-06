﻿using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml;

namespace Doc2web.Core
{
    public class RootElementContext : INestableElementContext
    {
        private List<HtmlNode> _nodes;
        private List<Mutation> _transformations;

        public RootElementContext(IGlobalContext context, OpenXmlElement rootElement)
        {
            GlobalContext = context;
            RootElement = rootElement;
            _nodes = new List<HtmlNode>();
            _transformations = new List<Mutation>();
        }

        public IGlobalContext GlobalContext { get; private set; }

        public OpenXmlElement RootElement { get; private set; }

        public IEnumerable<HtmlNode> Nodes => _nodes;

        public IEnumerable<Mutation> Mutations => _transformations;

        public OpenXmlElement Element => RootElement;

        public int TextIndex => 0;

        public IContextNestingHandler NestingHandler { get; set; }

        public void AddNode(HtmlNode node) => _nodes.Add(node);

        public void AddMultipleNodes(IEnumerable<HtmlNode> nodes) => _nodes.AddRange(nodes);

        public void AddMutation(Mutation transformation) =>
            _transformations.Add(transformation);

        public void AddMutations(IEnumerable<Mutation> transformations) =>
            _transformations.AddRange(transformations);

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

    }
}
