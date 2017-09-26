using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Doc2web.Core;

namespace Doc2web.Tests.Core
{
    [TestClass]
    public class RootElementContextTests
    {
        private OpenXmlElement _rootElement;
        private IGlobalContext _globalContext;
        private RootElementContext _instance;

        [TestInitialize]
        public void Initialize()
        {
            _rootElement = BuildParagraph();
            _globalContext = Substitute.For<IGlobalContext>();
            _instance = new RootElementContext(_globalContext, _rootElement);
        }

        [TestMethod]
        public void RootElementContext_Test()
        {
            Assert.AreSame(_rootElement, _instance.RootElement);
            Assert.AreSame(_rootElement, _instance.Element);
            Assert.IsNull(_instance.NestingHandler);
            Assert.AreEqual(0, _instance.Nodes.Count());
            Assert.AreEqual(0, _instance.Mutations.Count());
        }

        [TestMethod]
        public void AddNode_Test()
        {
            var node = new HtmlNode();

            _instance.AddNode(node);

            Assert.AreSame(node, _instance.Nodes.Single());
        }

        [TestMethod]
        public void AddMultiple_NodeTest()
        {
            var nodes = new HtmlNode[] { new HtmlNode(), new HtmlNode() };

            _instance.AddMultipleNodes(nodes);

            Assert.AreEqual(nodes.Length, _instance.Nodes.Count());
            foreach (var node in nodes)
                Assert.IsTrue(_instance.Nodes.Contains(node));
        }


        [TestMethod]
        public void AddTransformation_Test()
        {
            var transformation = Substitute.For<Mutation>();

            _instance.AddMutation(transformation);

            Assert.AreSame(transformation, _instance.Mutations.Single());
        }

        [TestMethod]
        public void AddMultipleTransformations_Test()
        {
            var transformations = new Mutation[] {
                Substitute.For<Mutation>(),
                Substitute.For<Mutation>()
            };

            _instance.AddMutations(transformations);

            Assert.AreEqual(transformations.Length, _instance.Mutations.Count());
            foreach (var transformation in transformations)
                Assert.IsTrue(_instance.Mutations.Contains(transformation));
        }

        [TestMethod]
        public void ProcessChildren_Test()
        {
            var nestingHandler = Substitute.For<IContextNestingHandler>();

            _instance.NestingHandler = nestingHandler;
            _instance.ProcessChilden();

            nestingHandler.Received(1).QueueElementProcessing(
                Arg.Is<ChildElementContext>(context =>
                    context.Element == _instance.Element.FirstChild
                ));
        }

        public class Test { }

        [TestMethod]
        public void Resolve_Test()
        {
            var t = new Test();
            _globalContext.Resolve<Test>().Returns(t);
            _globalContext.ClearReceivedCalls();

            var t2 = _instance.Resolve<Test>();

            Assert.AreSame(t, t2);
            _globalContext.Received(1).Resolve<Test>();
        }

        private static Paragraph BuildParagraph() =>
            new Paragraph(new Run(new Text("Sample text.")));
    }
}
