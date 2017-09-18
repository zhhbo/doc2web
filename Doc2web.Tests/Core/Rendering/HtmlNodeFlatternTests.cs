using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Doc2web.Core.Rendering;
using Doc2web.Core.Rendering.Step1;
using System.Linq;

namespace Doc2web.Tests.Core.Rendering
{
    [TestClass]
    public class HtmlNodeFlatternTests
    {
        private List<HtmlNode> ColisionEndInput =>
            new List<HtmlNode>
            {
                new HtmlNode { Start = 0, End = 10, Z = 100, Tag="div" },
                new HtmlNode { Start = 5, End = 15, Z = 000, Tag="section" }
            };

        private List<HtmlNode> ColisionEndExpected =>
            new List<HtmlNode>
            {
                new HtmlNode { Start = 00, End = 10, Z = 100, Tag="div" },
                new HtmlNode { Start = 05, End = 10, Z = 000, Tag="section" },
                new HtmlNode { Start = 10, End = 15, Z = 000, Tag="section" }
            };

        private List<HtmlNode> ColisionStartInput =>
            new List<HtmlNode>
            {
                new HtmlNode { Start = 05, End = 15, Z=100, Tag="div" },
                new HtmlNode { Start = 00, End = 10, Z=000, Tag="section" }
            };

        private List<HtmlNode> ColisionStartExpected =>
            new List<HtmlNode>
            {
                new HtmlNode { Start = 05, End = 15, Z=100, Tag="div" },
                new HtmlNode { Start = 00, End = 05, Z=000, Tag="section" },
                new HtmlNode { Start = 05, End = 10, Z=000, Tag="section" },
            };

        private List<HtmlNode> SurroundingInput =>
            new List<HtmlNode>
            {
                new HtmlNode { Start = 05, End = 10, Z=100, Tag="div" },
                new HtmlNode { Start = 00, End = 15, Z=000, Tag="section" }
            };

        private List<HtmlNode> SurrouningExpected =>
            new List<HtmlNode>
            {
                new HtmlNode { Start = 05, End = 10, Z=100, Tag="div" },
                new HtmlNode { Start = 00, End = 05, Z=000, Tag="section" },
                new HtmlNode { Start = 05, End = 10, Z=000, Tag="section" },
                new HtmlNode { Start = 10, End = 15, Z=000, Tag="section" },
            };

        private List<HtmlNode> UnaffectedNodesInput =>
            new List<HtmlNode>
            {
                new HtmlNode { Z = 100, Start = 00, End = 20, },
                new HtmlNode { Z = 000, Start = 00, End = 05, }, 
                new HtmlNode { Z = 000, Start = 07, End = 13, }, 
                new HtmlNode { Z = 000, Start = 15, End = 20, }, 
            };

        private List<HtmlNode> UnaffectedNodesExpected =>
            UnaffectedNodesInput;

        private List<HtmlNode> MultipleLayerInput2 =>
            new List<HtmlNode>
            {
                new HtmlNode { Z = 100, Start = 02, End = 09 },
                new HtmlNode { Z = 100, Start = 09, End = 18 },
                new HtmlNode { Z = 000, Start = 00, End = 04 },
                new HtmlNode { Z = 000, Start = 06, End = 12 },
                new HtmlNode { Z = 000, Start = 16, End = 20 }
            };

        private List<HtmlNode> MultipleLayerExpected2 =>
            new List<HtmlNode>
            {
                new HtmlNode { Z = 100, Start = 02, End = 09 },
                new HtmlNode { Z = 100, Start = 09, End = 18 },
                new HtmlNode { Z = 000, Start = 00, End = 02 },
                new HtmlNode { Z = 000, Start = 02, End = 04 },
                new HtmlNode { Z = 000, Start = 06, End = 09 },
                new HtmlNode { Z = 000, Start = 09, End = 12 },
                new HtmlNode { Z = 000, Start = 16, End = 18 },
                new HtmlNode { Z = 000, Start = 18, End = 20 }
            };

        private List<HtmlNode> TrippleLayerInput =>
            new List<HtmlNode>
            {
                new HtmlNode { Z = 100, Start = 00, End = 10 },
                new HtmlNode { Z = 100, Start = 10, End = 20 },
                new HtmlNode { Z = 050, Start = 07, End = 13 },
                new HtmlNode { Z = 000, Start = 00, End = 03 },
                new HtmlNode { Z = 000, Start = 05, End = 09 },
                new HtmlNode { Z = 000, Start = 09, End = 11 },
                new HtmlNode { Z = 000, Start = 11, End = 15 }
            };

        private List<HtmlNode> TrippleLayerExpected =>
            new List<HtmlNode>
            {
                new HtmlNode { Z = 100, Start = 00, End = 10 },
                new HtmlNode { Z = 100, Start = 10, End = 20 },
                new HtmlNode { Z = 050, Start = 07, End = 10 },
                new HtmlNode { Z = 050, Start = 10, End = 13 },
                new HtmlNode { Z = 000, Start = 00, End = 03 },
                new HtmlNode { Z = 000, Start = 05, End = 07 },
                new HtmlNode { Z = 000, Start = 07, End = 09 },
                new HtmlNode { Z = 000, Start = 09, End = 10 },
                new HtmlNode { Z = 000, Start = 10, End = 11 },
                new HtmlNode { Z = 000, Start = 11, End = 13 },
                new HtmlNode { Z = 000, Start = 13, End = 15 }
            };

        [TestMethod]
        public void Flattern_ColisionEndTest()
        {
            Test(ColisionEndInput, ColisionEndExpected);
        }

        [TestMethod]
        public void Flattern_ColisionStartTest()
        {
            Test(ColisionStartInput, ColisionStartExpected);
        }

        [TestMethod]
        public void Flattern_SurroundingTest()
        {
            Test(SurroundingInput, SurrouningExpected);
        }

        [TestMethod]
        public void Flattern_UnaffectedTest()
        {
            Test(UnaffectedNodesInput, UnaffectedNodesExpected);
        }

        [TestMethod]
        public void Flattern_MultipleLayerTest2()
        {
            Test(MultipleLayerInput2, MultipleLayerExpected2);
        }

        [TestMethod]
        public void Flattern_TrippleLayerTest()
        {
            Test(TrippleLayerInput, TrippleLayerExpected);
        }

        private void Test(List<HtmlNode> input, List<HtmlNode> expected)
        {
            var result = input.Select(x => x.Clone()).ToList();
            FlatternHtmlNodes.Apply(result);

            Assert.AreEqual(expected.Count, result.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                var r = result[i];
                var e = expected[i];

                Assert.AreEqual(e, r);
            }
        }
    }
}
