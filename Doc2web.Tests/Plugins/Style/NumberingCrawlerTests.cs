using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using Doc2web.Plugins.Style;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Linq;
using Doc2web.Tests.Samples;

namespace Doc2web.Tests.Plugins.Style
{
    [TestClass]
    public class NumberingCrawlerTests
    {
        private WordprocessingDocument _wpDoc;
        private DocumentFormat.OpenXml.Wordprocessing.Numbering _numbering;
        private Styles _styles;
        private NumberingCrawler _instance;

        [TestInitialize]
        public void Initialize()
        {
            _wpDoc = NumberingSample3.BuildDoc();
            _numbering = _wpDoc.MainDocumentPart.NumberingDefinitionsPart.Numbering;
            _styles = _wpDoc.MainDocumentPart.StyleDefinitionsPart.Styles;
            _instance = new NumberingCrawler(_numbering, _styles);
        }

        [TestMethod]
        public void Collect_SingleTest()
        {
            var results = _instance.Collect(10, 2);

            Assert.AreEqual(1, results.Count);
            Assert.AreSame(GetFromAnum(1, 2), results[0]);
        }

        [TestMethod]
        public void Collect_OverrideTest()
        {
            var results = _instance.Collect(10, 0);

            Assert.AreEqual(2, results.Count);
            Assert.AreSame(GetFromAnum(1, 0), results[0]);
            Assert.AreSame(GetFromNumI(10, 0), results[1]);
        }

        [TestMethod]
        public void Collect_StyleLinkTest()
        {
            var results = _instance.Collect(20, 2);

            Assert.AreEqual(1, results.Count);
            Assert.AreSame(GetFromAnum(1, 2), results[0]);
        }

        [TestMethod]
        public void Collect_StyleLinkOverrideTest()
        {
            var results = _instance.Collect(20, 0);

            Assert.AreEqual(2, results.Count);
            Assert.AreSame(GetFromAnum(1, 0), results[0]);
            Assert.AreSame(GetFromNumI(20, 0), results[1]);
        }

        [TestMethod]
        public void Collect_RecursiveTest()
        {
            Assert.ThrowsException<InvalidOperationException>(() => _instance.Collect(30, 0));
        }

        private Level GetFromAnum(int anumId, int level) =>
            _numbering
                .Elements<AbstractNum>()
                .Single(x => x.AbstractNumberId == anumId)
                .Elements<Level>()
                .Single(x => x.LevelIndex == level);

        private Level GetFromNumI(int numI, int level) =>
            _numbering
                .Elements<NumberingInstance>()
                .Single(x => x.NumberID == numI)
                .Elements<LevelOverride>()
                .Single(x => x.LevelIndex == level)
                .Level;
    }
}
