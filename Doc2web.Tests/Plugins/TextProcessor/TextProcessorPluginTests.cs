using Autofac;
using Doc2web.Core;
using Doc2web.Plugins.Style;
using Doc2web.Plugins.TextProcessor;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Tests.Plugins.TextProcessor
{
    [TestClass]
    public class TextProcessorPluginTests
    {
        private TextProcessorPlugin _instance;
        private Run _r;
        private ContainerBuilder _containerBuilder;
        private IContainer _container;
        private ILifetimeScope _lifetimeScope;
        private IGlobalContext _globalContext;
        private RootElementContext _pContext;
        private ChildElementContext _rContext;
        private Paragraph _p;
        private IContextNestingHandler _nestingHandler;
        private ICssRegistrator _cssRegistrator;

        [TestInitialize]
        public void Initialize()
        {
            _instance = new TextProcessorPlugin();
            _r = new Run(new Text("Some text."));
            _p = new Paragraph(_r);
            _nestingHandler = Substitute.For<IContextNestingHandler>();
            _cssRegistrator = Substitute.For<ICssRegistrator>();

            _containerBuilder = new ContainerBuilder();
            _containerBuilder.RegisterInstance(_cssRegistrator).As<ICssRegistrator>().SingleInstance();
            _container = _containerBuilder.Build();
            _lifetimeScope = _container.BeginLifetimeScope();

            _globalContext = Substitute.For<IGlobalContext>();
            _globalContext.Container.Returns(_lifetimeScope);

            _pContext = new RootElementContext(_globalContext, _p)
            {
                NestingHandler = _nestingHandler
            };
            _rContext = new ChildElementContext(_pContext)
            {
                Element = _r
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            _lifetimeScope.Dispose();
            _container.Dispose();
        }

        [TestMethod]
        public void TextProcessorPlugin_Test()
        {
            Assert.AreEqual("div", _instance.ContainerTag);
            Assert.AreEqual("container", _instance.ContainerCls);
            Assert.AreEqual(1_000_000, _instance.ContainerZ);

            Assert.AreEqual("div", _instance.IdentationTag);
            Assert.AreEqual("leftspacer", _instance.LeftIdentationCls);
            Assert.AreEqual("rightspacer", _instance.RightIdentationCls);

            Assert.AreEqual("p", _instance.ParagraphTag);
            Assert.AreEqual("", _instance.ParagraphCls);
            Assert.AreEqual(1_000, _instance.ParagraphZ);

            Assert.AreEqual("span", _instance.RunTag);
            Assert.AreEqual("", _instance.RunCls);
            Assert.AreEqual(1, _instance.RunZ);
        }

        [TestMethod]
        public void ProcessParagraph_AddContainerTest()
        {
            _instance.ProcessParagraph(_pContext, _p);

            var firstNode = _pContext.Nodes.First();
            var expected = new HtmlNode
            {
                Tag = _instance.ContainerTag,
                Start = 0,
                End = _p.InnerText.Length,
                Z = _instance.ContainerZ,
            };
            expected.AddClass(_instance.ContainerCls);
            Assert.AreEqual(expected, firstNode);
        }

        [TestMethod]
        public void ProcessParagraph_AddLeftIdentationsTest()
        {
            _instance.ProcessParagraph(_pContext, _p);

            var firstNode = _pContext.Nodes.ElementAt(1);
            var expected = new HtmlNode
            {
                Tag = _instance.IdentationTag,
                Start = 0,
                End = 0,
                Z = _instance.ParagraphZ,
            };
            expected.AddClass(_instance.LeftIdentationCls);
            Assert.AreEqual(expected, firstNode);
        }

        [TestMethod]
        public void ProcessParagraph_AddRightIdentationsTest()
        {
            _instance.ProcessParagraph(_pContext, _p);

            var firstNode = _pContext.Nodes.ElementAt(3);
            var expected = new HtmlNode
            {
                Tag = _instance.IdentationTag,
                Start = _p.InnerText.Length,
                End = _p.InnerText.Length,
                Z = _instance.ParagraphZ,
            };
            expected.AddClass(_instance.RightIdentationCls);
            Assert.AreEqual(expected, firstNode);
        }

        [TestMethod]
        public void ProcessParagraph_AddParagraphTest()
        {
            _instance.ProcessParagraph(_pContext, _p);

            var firstNode = _pContext.Nodes.ElementAt(2);
            var expected = new HtmlNode
            {
                Tag = _instance.ParagraphTag,
                Start = 0,
                End = _p.InnerText.Length,
                Z = _instance.ParagraphZ,
            };
            expected.AddClass(_instance.ParagraphCls);
            Assert.AreEqual(expected, firstNode);
        }

        [TestMethod]
        public void ProcessParagraph_AddDynStyleTest()
        {
            string styleName = "dync-somestuff";
            var pPr = new ParagraphProperties();
            _cssRegistrator.Register(pPr).Returns(styleName);
            _p.InsertAt(pPr, 0);

            _instance.ProcessParagraph(_pContext, _p);
            var node = _pContext.Nodes.First();

            node.Classes.Contains(styleName);
        }

        [TestMethod]
        public void ProcessParagraph_AddStyleIdTest()
        {
            string styleName = "heading1";
            var pPr = new ParagraphProperties(new StyleId() { Val = new StringValue(styleName) });
            _cssRegistrator.Register(pPr).Returns("");
            _cssRegistrator.Register(styleName).Returns(styleName);
            _p.InsertAt(pPr, 0);

            _instance.ProcessParagraph(_pContext, _p);
            var node = _pContext.Nodes.First();

            node.Classes.Contains(styleName);
        }

        [TestMethod]
        public void ProcessRun_NodeTest()
        {
            _instance.ProcessRun(_rContext, _r);

            var firstNode = _rContext.Nodes.Single();
            var expected = new HtmlNode
            {
                Tag = _instance.RunTag,
                Start = 0,
                End = _r.InnerText.Length,
                Z = _instance.RunZ,
            };
            expected.AddClass(_instance.RunCls);
            Assert.AreEqual(expected, firstNode);
        }

        [TestMethod]
        public void ProcessRun_DynStyleTest()
        {
            string styleName = "dync-somestuff";
            var rPr = new RunProperties();
            _cssRegistrator.Register(rPr).Returns(styleName);
            _r.InsertAt(rPr, 0);

            _instance.ProcessRun(_rContext, _r);
            var node = _rContext.Nodes.First();

            node.Classes.Contains(styleName);
        }

        [TestMethod]
        public void ProcessRun_StyleIdTest()
        {
            string styleName = "somestyle";
            var rPr = new RunProperties(new StyleId() { Val = styleName });
            _cssRegistrator.Register(rPr).Returns("");
            _cssRegistrator.Register(styleName).Returns(styleName);
            _r.InsertAt(rPr, 0);

            _instance.ProcessRun(_rContext, _r);
            var node = _rContext.Nodes.First();

            node.Classes.Contains(styleName);
        }
    }
}
