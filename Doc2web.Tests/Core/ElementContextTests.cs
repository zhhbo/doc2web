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
    public class ElementContextTests
    {
        private OpenXmlElement _rootElement;
        private IGlobalContext _globalContext;
        private ElementContext _instance;

        [TestInitialize]
        public void Initialize()
        {
            _rootElement = BuildParagraph();
            _globalContext = Substitute.For<IGlobalContext>();
            _instance = new ElementContext(_globalContext, _rootElement);
        }

        [TestMethod]
        public void ElementContext_Test()
        {
            Assert.AreSame(_globalContext, _instance.GlobalContext);
            Assert.AreSame(_rootElement, _instance.RootElement);
            Assert.AreEqual(_rootElement.InnerText, _instance.RootElementText);
            Assert.AreEqual(0, _instance.Nodes.Count());
            Assert.AreEqual(0, _instance.Transformations.Count());
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
            var transformation = Substitute.For<ITextTransformation>();

            _instance.AddTranformation(transformation);

            Assert.AreSame(transformation, _instance.Transformations.Single());
        }

        [TestMethod]
        public void AddMultipleTransformations_Test()
        {
            var transformations = new ITextTransformation[] {
                Substitute.For<ITextTransformation>(),
                Substitute.For<ITextTransformation>()
            };

            _instance.AddMultipleTransformations(transformations);

            Assert.AreEqual(transformations.Length, _instance.Transformations.Count());
            foreach (var transformation in transformations)
                Assert.IsTrue(_instance.Transformations.Contains(transformation));
        }
        private static Paragraph BuildParagraph() =>
            new Paragraph(new Run(new Text("Sample text.")));
    }
}
