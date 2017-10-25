using Autofac;
using Doc2web.Plugins.TableOfContent;
using Doc2web.Tests.Samples;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Tests.Plugins.TableOfContent
{
    [TestClass]
    public class TableOfContentPluginTests
    {
        private TableOfContentConfig _config;
        private TableOfContentPlugin _instance;
        private HtmlNode _pNode;
        private IElementContext _elementContext;

        [TestInitialize]
        public void Initialize()
        {
            _config = new TableOfContentConfig();
            _instance = new TableOfContentPlugin(_config);
            _pNode = new HtmlNode { Start = 0, Tag = "p" };
            _elementContext = Substitute.For<IElementContext>();
            _elementContext.Nodes.Returns(new HtmlNode[] { _pNode });
        }

        [TestMethod]
        public void RegisterConfig_Test()
        {
            var containerBuilder = new ContainerBuilder();

            _instance.RegisterConfig(containerBuilder);
            var container = containerBuilder.Build();
            var config = container.Resolve<TableOfContentConfig>();

            Assert.AreSame(_config, config);
        }

        [TestMethod]
        public void ProcessHyperlink_NotLastChildTest()
        {
            var a = new Hyperlink(new Run(new Text("link")));
            var p = new Paragraph(
                new Run(new Text("here is a ")),
                a,
                new Run(new Text(".")));
            _elementContext.RootElement.Returns(p);
            _elementContext.Element.Returns(a);

            _instance.ProcessHyperlink(_elementContext, a);

            AssertDidNotMutateContext();
        }

        [TestMethod]
        public void ProcessHyperlink_NotTOCTest()
        {
            var a = new Hyperlink(new Run(new Text("link")));
            var p = new Paragraph(
                new Run(new Text("This is not a table of content: ")),
                a);
            _elementContext.RootElement.Returns(p);
            _elementContext.Element.Returns(a);

            _instance.ProcessHyperlink(_elementContext, a);

            AssertDidNotMutateContext();
        }

        [TestMethod]
        public void ProcessHyperlink_RemoveAllTextBeforeTest()
        {
            TestSample1();

            var m = GetMutations().First() as TextDeletion;
            Assert.IsNotNull(m);
            Assert.AreEqual(0, m.Position);
            Assert.AreEqual(_elementContext.TextIndex, m.Count);
        }

        [TestMethod]
        public void ProcessHyperlink_ReplaceCrossRefBySpacerTest()
        {
            int tocCrossRefIndex = 64;
            TestSample1();

            AssertCrossRefDeleted();
            AssertHasTocSpacer(tocCrossRefIndex);
        }

        [TestMethod]
        public void ProcessHyperlink_AddCssClassToParagraphTest()
        {
            TestSample1();

            AssertContainerHasTOCClass();
        }

        [TestMethod]
        public void ProcessHyperlink_ProcessChildrenTest ()
        {
            TestSample1();

            _elementContext.Received(1).ProcessChilden();
        }

        [TestMethod]
        public void PostProcessing_Test()
        {
            var context = Substitute.For<IGlobalContext>();
            _instance.PostProcessing(context);

            context.Received(1).AddCss(TableOfContentPlugin.CSS(_config.ParagraphTocCssClass, _config.SpacerCssClass));
        }

        private void AssertContainerHasTOCClass()
        {
            Assert.IsTrue(_pNode.Classes.Contains(_config.ParagraphTocCssClass));
        }

        private void AssertCrossRefDeleted()
        {
            var m = GetMutations().Last() as TextDeletion;
            Assert.IsNotNull(m);
            Assert.AreEqual(64, m.Position);
            Assert.AreEqual(25, m.Count);
        }

        private void AssertHasTocSpacer(int tocCrossRefIndex)
        {
            var expected = new HtmlNode()
            {
                Start = tocCrossRefIndex,
                End = tocCrossRefIndex,
                Tag = _config.SpacerTag,
                Z = _config.SpacerZ
            };
            expected.AddClasses(_config.SpacerCssClass);
            var node = GetNodes()
                .First(x => x.Start == tocCrossRefIndex && x.End == tocCrossRefIndex);
            Assert.AreEqual(expected, node);
        }

        private IEnumerable<HtmlNode> GetNodes() => 
            _elementContext.ReceivedCalls()
                .Where(x => x.GetMethodInfo().Name == nameof(_elementContext.AddNode))
                .Select(x => x.GetArguments()[0] as HtmlNode);

        private IEnumerable<Mutation> GetMutations() => 
            _elementContext.ReceivedCalls()
                .Where(x => x.GetMethodInfo().Name == nameof(_elementContext.AddMutation))
                .Select(x => x.GetArguments()[0] as Mutation);

        private void TestSample1()
        {
            var (context, hyperlink) = TOCSample1.GenerateProcessingArgs();
            _elementContext = context;
            _elementContext.Nodes.Returns(new HtmlNode[] { _pNode });
            _instance.ProcessHyperlink(_elementContext, hyperlink);
        }

        private void AssertDidNotMutateContext()
        {
            _elementContext.DidNotReceiveWithAnyArgs().AddNode(null);
            _elementContext.DidNotReceiveWithAnyArgs().AddMutation(null);
            _elementContext.DidNotReceiveWithAnyArgs().AddMultipleNodes(null);
            _elementContext.DidNotReceiveWithAnyArgs().AddMutations(null);
        }
    }
}
