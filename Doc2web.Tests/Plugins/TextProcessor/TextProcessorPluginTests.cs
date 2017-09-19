﻿using Autofac;
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
        private TextProcessorConfig _config;
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
            _config = new TextProcessorConfig();
            _instance = new TextProcessorPlugin(_config);
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
            Assert.AreEqual("div", _config.ContainerTag);
            Assert.AreEqual("container", _config.ContainerCls);
            Assert.AreEqual(1_000_000, _config.ContainerZ);

            Assert.AreEqual("div", _config.IdentationTag);
            Assert.AreEqual("leftspacer", _config.LeftIdentationCls);
            Assert.AreEqual("rightspacer", _config.RightIndentationCls);

            Assert.AreEqual("p", _config.ParagraphTag);
            Assert.AreEqual("", _config.ParagraphCls);
            Assert.AreEqual(1_000, _config.ParagraphZ);

            Assert.AreEqual("span", _config.RunTag);
            Assert.AreEqual("", _config.RunCls);
            Assert.AreEqual(1, _config.RunZ);
        }

        [TestMethod]
        public void ProcessParagraph_AddContainerTest()
        {
            _instance.ProcessParagraph(_pContext, _p);

            var firstNode = _pContext.Nodes.First();
            var expected = new HtmlNode
            {
                Tag = _config.ContainerTag,
                Start = _config.ContainerStart,
                End = _config.ContainerEnd,
                Z = _config.ContainerZ,
            };
            expected.AddClass(_config.ContainerCls);
            Assert.AreEqual(expected, firstNode);
        }

        [TestMethod]
        public void ProcessParagraph_AddLeftIdentationsTest()
        {
            _instance.ProcessParagraph(_pContext, _p);

            var firstNode = _pContext.Nodes.ElementAt(1);
            var expected = new HtmlNode
            {
                Tag = _config.IdentationTag,
                Start = _config.ContainerStart + _config.Delta,
                End = _config.ContainerStart + _config.Delta * 2,
                Z = _config.ParagraphZ,
            };
            expected.AddClass(_config.LeftIdentationCls);
            Assert.AreEqual(expected, firstNode);
        }

        [TestMethod]
        public void ProcessParagraph_AddRightIdentationsTest()
        {
            _instance.ProcessParagraph(_pContext, _p);

            var firstNode = _pContext.Nodes.ElementAt(3);
            var expected = new HtmlNode
            {
                Tag = _config.IdentationTag,
                Start = _config.ContainerEnd - _config.Delta * 2,
                End = _config.ContainerEnd - _config.Delta,
                Z = _config.ParagraphZ,
            };
            expected.AddClass(_config.RightIndentationCls);
            Assert.AreEqual(expected, firstNode);
        }

        [TestMethod]
        public void ProcessParagraph_AddParagraphTest()
        {
            _instance.ProcessParagraph(_pContext, _p);

            var firstNode = _pContext.Nodes.ElementAt(2);
            var expected = new HtmlNode
            {
                Tag = _config.ParagraphTag,
                Start = 0 - _config.Delta,
                End = _p.InnerText.Length + _config.Delta,
                Z = _config.ParagraphZ,
            };
            expected.AddClass(_config.ParagraphCls);
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
                Tag = _config.RunTag,
                Start = _config.Delta,
                End = _r.InnerText.Length,
                Z = _config.RunZ,
            };
            expected.AddClass(_config.RunCls);
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