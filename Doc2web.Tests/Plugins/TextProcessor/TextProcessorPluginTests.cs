using Autofac;
using Doc2web.Core;
using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Css;
using Doc2web.Plugins.Style.Properties;
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
            _globalContext = Substitute.For<IGlobalContext>();
            _globalContext.Resolve<ICssRegistrator>().Returns(_cssRegistrator);
            _cssRegistrator.RegisterParagraph(null, null).ReturnsForAnyArgs(x => new CssClass());
            _cssRegistrator.RegisterRun(null, null, null).ReturnsForAnyArgs(x => new CssClass());

            _pContext = new RootElementContext(_globalContext, _p)
            {
                NestingHandler = _nestingHandler
            };
            _rContext = new ChildElementContext(_pContext)
            {
                Element = _r
            };
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
            expected.AddClasses(_config.ContainerCls);
            Assert.AreEqual(expected, firstNode);
        }

        [TestMethod]
        public void ProcessParagraph_AddLeftIdentationsTest()
        {
            var indentation = new Indentation { Left = "100" };
            MockIndentation(indentation, "dyn-class");

            _instance.ProcessParagraph(_pContext, _p);

            var spacer = _pContext.Nodes.ElementAt(0);
            var expected = new HtmlNode
            {
                Tag = _config.IdentationTag,
                Start = _config.ContainerStart + _config.Delta,
                End = _config.ContainerStart + _config.Delta * 2,
                Z = _config.ParagraphZ,
            };
            expected.AddClasses(_config.LeftIdentationCls);
            Assert.AreEqual(expected, spacer);
        }


        [TestMethod]
        public void ProcessParagraph_AddRightIdentationsTest()
        {
            MockIndentation(new Indentation { Right = "100" }, "dyn-class");
            _instance.ProcessParagraph(_pContext, _p);

            var spacer = _pContext.Nodes.ElementAt(0);
            var expected = new HtmlNode
            {
                Tag = _config.IdentationTag,
                Start = _config.ContainerEnd - _config.Delta * 2,
                End = _config.ContainerEnd - _config.Delta,
                Z = _config.ParagraphZ,
            };
            expected.AddClasses(_config.RightIndentationCls);
            Assert.AreEqual(expected, spacer);
        }

        [TestMethod]
        public void ProcessParagraph_AddParagraphTest()
        {
            _instance.ProcessParagraph(_pContext, _p);

            var firstNode = _pContext.Nodes.ElementAt(1);
            var expected = new HtmlNode
            {
                Tag = _config.ParagraphTag,
                Start = 0 - _config.Delta,
                End = _p.InnerText.Length + _config.Delta,
                Z = _config.ParagraphZ,
            };
            expected.AddClasses(_config.ParagraphCls);
            Assert.AreEqual(expected, firstNode);
        }

        [TestMethod]
        public void ProcessParagraph_AddStyleTest()
        {
            string styleName = "dyn-somestuff";
            var pPr = new ParagraphProperties();
            _cssRegistrator
                .RegisterParagraph(pPr)
                .Returns(new CssClass { Name = styleName });
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
            expected.AddClasses(_config.RunCls);
            Assert.AreEqual(expected, firstNode);
        }

        [TestMethod]
        public void ProcessRun_StyleTest()
        {
            string styleName = "dyn-somestuff";
            var rPr = new RunProperties();
            _cssRegistrator
                .RegisterRun((_rContext.RootElement as Paragraph).ParagraphProperties, rPr, null)
                .Returns(new CssClass { Name = styleName });
            _r.InsertAt(rPr, 0);

            _instance.ProcessRun(_rContext, _r);
            var node = _rContext.Nodes.First();

            node.Classes.Contains(styleName);
        }
        private void MockIndentation(Indentation indentation, string clsName)
        {
            _p.ParagraphProperties = new ParagraphProperties();
            _cssRegistrator
                .RegisterParagraph(Arg.Is(_p.ParagraphProperties), null)
                .Returns(x => new CssClass
                {
                    Name = clsName,
                    Props = new CssPropertiesSet {
                        new IndentationCssProperty(null)
                        {
                            Element = indentation
                        }
                    }
                });
        }
    }
}
