using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Whusion.Core.Rendering;

namespace Whusion.Tests.Core.Rendering
{
    [TestClass]
    public class HtmlNodeFlatternTests
    {
        private List<HtmlNode> SingleLayerInput1 =>
            new List<HtmlNode>
            {
                new HtmlNode { Start = 0, End = 10, Tag="div" },
                new HtmlNode { Start = 5, End = 15, Tag="section" }
            };

        private List<HtmlNode> SingleLayerExpected1 =>
            new List<HtmlNode>
            {
                new HtmlNode { Start = 0, End = 5, Tag="div" },
                new HtmlNode { Start = 5, End = 10, Tag="div" },
                new HtmlNode { Start = 5, End = 10, Tag="section" },
                new HtmlNode { Start = 10, End = 15, Tag="section" }

            };
        private List<HtmlNode> SingleLayerInput2 =>
            new List<HtmlNode>
            {
                new HtmlNode { Start = 0, End = 15, Tag="div" },
                new HtmlNode { Start = 5, End = 10, Tag="section" }
            };

        private List<HtmlNode> SingleLayerExpected2 =>
            new List<HtmlNode>
            {
                new HtmlNode { Start = 0, End = 5, Tag="div" },
                new HtmlNode { Start = 5, End = 10, Tag="div" },
                new HtmlNode { Start = 5, End = 10, Tag="section" },
                new HtmlNode { Start = 10, End = 15, Tag="div" }

            };

        private List<HtmlNode> SingleLayerInput3 =>
            new List<HtmlNode>
            {
                new HtmlNode { Start = 00, End = 15, Tag="a" },
                new HtmlNode { Start = 03, End = 09, Tag="b" },
                new HtmlNode { Start = 06, End = 12, Tag="c" },
            };

        private List<HtmlNode> SingleLayerExpected3 =>
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

        [TestMethod]
        public void Flattern_SingleLayerTest1()
        {
            Test(SingleLayerInput1, SingleLayerExpected1);
        }

        [TestMethod]
        public void Flattern_SingleLayerTest2()
        {
            Test(SingleLayerInput2, SingleLayerExpected2);
        }

        [TestMethod]
        public void Flattern_SingleLayerTest3()
        {
            Test(SingleLayerInput3, SingleLayerExpected3);
        }

        [TestMethod]
        public void Flattern_SingleLayerTest4()
        {
            Test(SingleLayerInput4, SingleLayerExpected4);
        }


        private void Test(List<HtmlNode> input, List<HtmlNode> expected)
        {
            var result = HtmlNodeFlattern.Flattern(input);

            Assert.AreEqual(expected.Count, result.Count);

            for(int i=0; i<expected.Count; i++)
            {
                var r = result[i];
                var e = expected[i];

                Assert.AreEqual(e, r);
            }
        }
    }
}
