using Doc2web.Plugins.TextFixes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Linq;
using System.Collections;
using Doc2web.Plugins.Numbering;

namespace Doc2web.Tests.Plugins.TextFixes
{
    [TestClass]
    public class BreakInsertionPluginTests
    {
        private BreakInsertionConfig _config;
        private BreakInsertionPlugin _instance;
        private List<HtmlNode> _nodes;
        private HtmlNode _numberingContainerMax;
        private IElementContext _context;
        private HtmlNode _container;

        [TestInitialize]
        public void Initialize()
        {
            _config = new BreakInsertionConfig();
            _instance = new BreakInsertionPlugin(_config);
            _nodes = new List<HtmlNode>();

            _container = new HtmlNode
            {
                Start = -1_000_000,
                End = -1_000_000,
                Z = _config.ContainerZ,
                Tag = _config.ContainerTag
            };
            _container.AddClasses(_config.ContainerCls);

            _context = Substitute.For<IElementContext>();
            _context.When(x => x.AddNode(Arg.Any<HtmlNode>()))
                .Do(x => _nodes.Add(x.ArgAt<HtmlNode>(0)));
            _context.TextIndex.Returns(100);
            _context.Nodes.Returns(new HtmlNode[] { _container, _numberingContainerMax });
        }

        [TestMethod]
        public void BrInsertion_Test()
        {
            _instance.BrInsertion(_context, new Break());
            AssertBrInserted();
        }

        [TestMethod]
        public void CrInsertion_Test()
        {
            _instance.CrInsertion(_context, new CarriageReturn());
            AssertBrInserted();
        }

        private void AssertBrInserted()
        {
            var node = _nodes.Single();
            Assert.AreEqual("br", node.Tag);
            Assert.AreEqual(100, node.Start);
        }

        [TestMethod]
        public void BrInsertion_TextIndex0Test()
        {
            _context.TextIndex.Returns(0);

            _instance.BrInsertion(_context, new Break());
            AssertContainerFlexIsCol();
        }

        [TestMethod]
        public void CrInsertion_TextIndex0Test()
        {
            _context.TextIndex.Returns(0);

            _instance.CrInsertion(_context, new CarriageReturn());
            AssertContainerFlexIsCol();
        }

        private void AssertContainerFlexIsCol()
        {
            Assert.AreEqual(0, _nodes.Count);
            Assert.IsTrue(_container.Classes.Contains(_config.BreakAtStartCls));
        }
    }
}
