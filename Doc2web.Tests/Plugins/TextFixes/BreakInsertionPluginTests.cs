using Doc2web.Plugins.TextFixes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Linq;

namespace Doc2web.Tests.Plugins.TextFixes
{
    [TestClass]
    public class BreakInsertionPluginTests
    {
        private BreakInsertionPlugin _instance;
        private List<HtmlNode> _nodes;
        private IElementContext _context;

        [TestInitialize]
        public void Initialize()
        {
            _instance = new BreakInsertionPlugin();
            _nodes = new List<HtmlNode>();
            _context = Substitute.For<IElementContext>();
            _context.When(x => x.AddNode(Arg.Any<HtmlNode>()))
                .Do(x => _nodes.Add(x.ArgAt<HtmlNode>(0)));
            _context.TextIndex.Returns(100);
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

    }
}
