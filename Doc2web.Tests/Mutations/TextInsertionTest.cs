using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Mutations
{
    [TestClass]
    public class TextInsertionTest
    {
        public static List<HtmlNode> NodeTest1_Input => new List<HtmlNode>
        {
            new HtmlNode { Start = 00, End = 09, Z = 10 },
            new HtmlNode { Start = 00, End = 03, Z = 00 },
            new HtmlNode { Start = 03, End = 06, Z = 00 },
            new HtmlNode { Start = 06, End = 09, Z = 00 },
        };

        public static List<HtmlNode> NodesTest1_Expected => new List<HtmlNode>
        {
            new HtmlNode { Start = 0, End = 12, Z = 10 },
            new HtmlNode { Start = 0, End = 03, Z = 00 },
            new HtmlNode { Start = 3, End = 09, Z = 00 },
            new HtmlNode { Start = 9, End = 12, Z = 00 },
        };

        [TestMethod]
        public void MutateNode_Test()
        {
            var mutation = new TextInsertion
            {
                Index = 5,
                Text = "123"
            };

            TestNodes(mutation, NodeTest1_Input, NodesTest1_Expected);
        }

        [TestMethod]
        public void MutateText_Test()
        {
            var input = new StringBuilder("...............");
            var mutation = new TextInsertion
            {
                Index = 5,
                Text = "123"
            };

            mutation.MutateText(input);

            Assert.AreEqual(".....123..........", input.ToString());
        }

        private void TestNodes(Mutation mutation,List<HtmlNode> input, List<HtmlNode> expected)
        {
            mutation.MutateNodes(input);
            var result = input;

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
