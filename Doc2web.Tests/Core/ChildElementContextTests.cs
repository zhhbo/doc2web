using Doc2web.Core;
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
        private IContextNestingHandler _nestingHandler;
        private INestableElementContext _parent;

        [TestInitialize]
        public void Initialize()
        {
            _nestingHandler = Substitute.For<IContextNestingHandler>();
            _parent = Substitute.For<INestableElementContext>();
            _parent.RootElement.Returns(BuildParagraph());
            _parent.Element.Returns(_parent.RootElement);
            _parent.Nodes.Returns(Enumerable.Empty<HtmlNode>());
            _parent.Mutations.Returns(Enumerable.Empty<Mutation>());
            _parent.NestingHandler.Returns(_nestingHandler);

            _instance = new ChildElementContext(_parent);
            _instance.Element = BuildParagraph();
        }

        [TestMethod]
        public void ChildElementContext_Test()
        {
            Assert.AreEqual(0, _instance.TextIndex);
            Assert.AreSame(_parent.RootElement, _instance.RootElement);
            Assert.AreSame(_parent.Nodes, _instance.Nodes);
            Assert.AreSame(_parent.Mutations, _instance.Mutations);
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
            var transfortions = new Mutation[] { };

            _instance.AddMutations(transfortions);

            _parent.Received(1).AddMutations(
                Arg.Is<IEnumerable<Mutation>>(transfortions));
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
            var transformation = Substitute.For<Mutation>();

            _instance.AddMutation(transformation);

            _parent.Received(1).AddMutation(Arg.Is(transformation));
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

        public class Test { }

        [TestMethod]
        public void Resolve_Test()
        {
            var t = new Test();
            _parent.Resolve<Test>().Returns(t);
            _parent.ClearReceivedCalls();

            var t2 = _instance.Resolve<Test>();

            Assert.AreSame(t, t2);
            _parent.Received(1).Resolve<Test>();
        }

        private static Paragraph BuildParagraph() =>
            new Paragraph(
                new Run(new Text("Sample text.")),
                new Run(new Text("Second sample text.")));
    }
}
