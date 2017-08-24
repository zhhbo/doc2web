﻿using Doc2web.Core;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Tests.Core
{
    [TestClass]
    public class ChildElementContextTests
    {
        private ChildElementContext _instance;
        private IGlobalContext _globalContext;
        private IContextNestingHandler _nestingHandler;
        private INestableElementContext _parent;

        [TestInitialize]
        public void Initialize()
        {
            _globalContext = Substitute.For<IGlobalContext>();
            _nestingHandler = Substitute.For<IContextNestingHandler>();
            _parent = Substitute.For<INestableElementContext>();
            _parent.GlobalContext.Returns(_globalContext);
            _parent.RootElement.Returns(BuildParagraph());
            _parent.Element.Returns(_parent.RootElement);
            _parent.Nodes.Returns(Enumerable.Empty<HtmlNode>());
            _parent.Transformations.Returns(Enumerable.Empty<ITextTransformation>());
            _parent.NestingHandler.Returns(_nestingHandler);

            _instance = new ChildElementContext(_parent);
            _instance.Element = BuildParagraph();
        }

        [TestMethod]
        public void ChildElementContext_Test()
        {
            Assert.AreSame(_globalContext, _instance.GlobalContext);
            Assert.AreEqual(0, _instance.TextIndex);
            Assert.AreSame(_parent.RootElement, _instance.RootElement);
            Assert.AreSame(_parent.Nodes, _instance.Nodes);
            Assert.AreSame(_parent.Transformations, _instance.Transformations);
            Assert.AreSame(_parent.NestingHandler, _instance.NestingHandler);
            Assert.AreNotSame(_parent.Element, _instance.Element);
        }

        [TestMethod]
        public void AddMultipleNodes_Test()
        {
            var nodes = new HtmlNode[] { };

            _instance.AddMultipleNodes(nodes);

            _parent.Received(1).AddMultipleNodes(Arg.Is<IEnumerable<HtmlNode>>(nodes));
        }

        [TestMethod]
        public void AddMultipleTransformations_Test()
        {
            var transfortions = new ITextTransformation[] { };

            _instance.AddMultipleTransformations(transfortions);

            _parent.Received(1).AddMultipleTransformations(
                Arg.Is<IEnumerable<ITextTransformation>>(transfortions));
        }

        [TestMethod]
        public void AddNode_Test()
        {
            var node = new HtmlNode();

            _instance.AddNode(node);

            _parent.Received(1).AddNode(Arg.Is(node));
        }

        [TestMethod]
        public void AddTransformation_Test()
        {
            var transformation = Substitute.For<ITextTransformation>();

            _instance.AddTranformation(transformation);

            _parent.Received(1).AddTranformation(Arg.Is(transformation));
        }

        [TestMethod]
        public void ProcesChildren_Test()
        {
            _instance.ProcessChilden();

            var nestedContexts = _nestingHandler.ReceivedCalls()
                .Select(x => x.GetArguments()[0])
                .Cast<ChildElementContext>()
                .ToArray();

            Assert.AreEqual(2, nestedContexts.Length);

            Assert.AreSame(_instance.Element.ChildElements[0], nestedContexts[0].Element);
            Assert.AreEqual(0, nestedContexts[0].TextIndex);
            
            Assert.AreSame(_instance.Element.ChildElements[1], nestedContexts[1].Element);
            Assert.AreEqual(_instance.Element.ChildElements[0].InnerText.Length, nestedContexts[1].TextIndex);
        }

        private static Paragraph BuildParagraph() =>
            new Paragraph(
                new Run(new Text("Sample text.")),
                new Run(new Text("Second sample text.")));
    }
}