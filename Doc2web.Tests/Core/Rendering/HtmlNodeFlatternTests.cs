﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        private List<HtmlNode> SingleLayerInput4 =>
            new List<HtmlNode>
            {
                new HtmlNode { Start = 00, End = 21, Tag="a" },
                new HtmlNode { Start = 03, End = 09, Tag="b" },
                new HtmlNode { Start = 06, End = 15, Tag="c" },
                new HtmlNode { Start = 12, End = 18, Tag="d" },
            };

        private List<HtmlNode> SingleLayerExpected4 =>
            new List<HtmlNode>
            {
                new HtmlNode { Start = 00, End = 03, Tag="a" },
                new HtmlNode { Start = 03, End = 06, Tag="a" },
                new HtmlNode { Start = 03, End = 06, Tag="b" },
                new HtmlNode { Start = 06, End = 09, Tag="a" },
                new HtmlNode { Start = 06, End = 09, Tag="b" },
                new HtmlNode { Start = 06, End = 09, Tag="c" },
                new HtmlNode { Start = 09, End = 12, Tag="a" },
                new HtmlNode { Start = 09, End = 12, Tag="c" },
                new HtmlNode { Start = 12, End = 15, Tag="a" },
                new HtmlNode { Start = 12, End = 15, Tag="c" },
                new HtmlNode { Start = 12, End = 15, Tag="d" },
                new HtmlNode { Start = 15, End = 18, Tag="a" },
                new HtmlNode { Start = 15, End = 18, Tag="d" },
                new HtmlNode { Start = 18, End = 21, Tag="a" }
            };

        private List<HtmlNode> MultipleLayerInput1 =>
            new List<HtmlNode>
            {
                new HtmlNode { Z = 100, Start = 00, End = 20, },
                new HtmlNode { Z = 000, Start = 00, End = 05, }, 
                new HtmlNode { Z = 000, Start = 07, End = 13, }, 
                new HtmlNode { Z = 000, Start = 15, End = 20, }, 
            };

        private List<HtmlNode> MultipleLayerExpected1 =>
            new List<HtmlNode>
            {
                new HtmlNode { Z = 100, Start = 00, End = 20, },
                new HtmlNode { Z = 000, Start = 00, End = 05, }, 
                new HtmlNode { Z = 000, Start = 07, End = 13, }, 
                new HtmlNode { Z = 000, Start = 15, End = 20, }, 
            };

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

        private List<HtmlNode> MultipleLayerInput3 =>
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

        private List<HtmlNode> MultipleLayerExpected3 =>
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
            TestPrototype(ColisionEndInput, ColisionEndExpected);
        }

        [TestMethod]
        public void Flattern_ColisionStartTest()
        {
            TestPrototype(ColisionStartInput, ColisionStartExpected);
        }

        [TestMethod]
        public void Flattern_SurroundingTest()
        {
            TestPrototype(SurroundingInput, SurrouningExpected);
        }

        [TestMethod]
        public void Flattern_SingleLayerTest4()
        {
            Test(SingleLayerInput4, SingleLayerExpected4);
        }

        [TestMethod]
        public void Flattern_MultipleLayerTest1()
        {
            Test(MultipleLayerInput1, MultipleLayerExpected1);
        }

        [TestMethod]
        public void Flattern_MultipleLayerTest2()
        {
            Test(MultipleLayerInput2, MultipleLayerExpected2);
        }

        [TestMethod]
        public void Flattern_MultipleLayerTest3()
        {
            Test(MultipleLayerInput3, MultipleLayerExpected3);
        }

        private void TestPrototype(List<HtmlNode> input, List<HtmlNode> expected)
        {
            var result = input.Select(x => x.Clone()).ToList();
            FlatternPrototype.Flattern(result);

            Assert.AreEqual(expected.Count, result.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                var r = result[i];
                var e = expected[i];

                Assert.AreEqual(e, r);
            }
        }

        private void Test(List<HtmlNode> input, List<HtmlNode> expected)
        {
            var result = FlatternHtmlNodes.Flattern(input);

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
