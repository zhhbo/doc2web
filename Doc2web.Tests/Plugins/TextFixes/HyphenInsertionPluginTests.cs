using Doc2web.Core;
using Doc2web.Plugins.TextFixes;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Tests.Plugins.TextFixes
{
    [TestClass]
    public class HyphenInsertionPluginTests
    {
        private HyphenInsertionPlugin _instance;
        private IElementContext _context;
        private List<TextInsertion> _textInsertions;

        [TestInitialize]
        public void Initialize()
        {
            _instance = new HyphenInsertionPlugin();
            _textInsertions = new List<TextInsertion>();
            _context = Substitute.For<IElementContext>();
            _context.TextIndex.Returns(666);
            _context.When(x => x.AddMutation(Arg.Any<Mutation>())).Do(x =>
            {
                if (x.ArgAt<Mutation>(0) is TextInsertion ti) _textInsertions.Add(ti);
            });
        }

        [TestMethod]
        public void InsertSoftHyphen_Test()
        {
            var dash = new SoftHyphen();

            _instance.InsertSoftHyphen(_context, dash);

            var insertion = _textInsertions.Single();
            Assert.AreEqual(666, insertion.Position);
            Assert.AreEqual("&#173;", insertion.Text);
        }

        [TestMethod]
        public void InsertNoBreakHyphen_Test()
        {
            var dash = new NoBreakHyphen();

            _instance.InsertNoBreakHyphen(_context, dash);

            var insertion = _textInsertions.Single();
            Assert.AreEqual(666, insertion.Position);
            Assert.AreEqual("&#8209;", insertion.Text);
        }
    }
}
