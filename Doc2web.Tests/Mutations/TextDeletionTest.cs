using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Mutations
{
    [TestClass]
    public class TextDeletionTest
    {
        public static List<HtmlNode> NodeTest1_Input => new List<HtmlNode>
        {
            new HtmlNode { Start = 00, End = 25, Z = 10, Tag = "np" },
            new HtmlNode { Start = 00, End = 05, Z = 00, Tag = "n0" },
            new HtmlNode { Start = 05, End = 10, Z = 00, Tag = "n1" },
            new HtmlNode { Start = 10, End = 15, Z = 00, Tag = "n2" },
            new HtmlNode { Start = 15, End = 20, Z = 00, Tag = "n3" },
            new HtmlNode { Start = 20, End = 25, Z = 00, Tag = "n4" },
        };

        public static List<HtmlNode> NodesTest1_Expected => new List<HtmlNode>
        {
            new HtmlNode { Start = 00, End = 16, Z = 10, Tag = "np" },
            new HtmlNode { Start = 00, End = 05, Z = 00, Tag = "n0" },
            new HtmlNode { Start = 05, End = 08, Z = 00, Tag = "n1" },
            new HtmlNode { Start = 08, End = 11, Z = 00, Tag = "n3" },
            new HtmlNode { Start = 11, End = 16, Z = 00, Tag = "n4" },
        };

        public static List<HtmlNode> NodeTest2_Input => new List<HtmlNode>
        {
            new HtmlNode { Start = 00, End = 10, Z = 10, Tag = "np" },
        };

        public static List<HtmlNode> NodesTest2_Expected => new List<HtmlNode>
        {
            new HtmlNode { Start = 00, End = 05, Z = 10, Tag = "np" },
        };

        [TestMethod]
        public void MutateNode_Test1()
        {
            var mutation = new TextDeletion { Index = 8, Length = 9 };
            TestNodes(mutation, NodeTest1_Input, NodesTest1_Expected);
        }

        [TestMethod]
        public void MutateNode_Test2()
        {
            var mutation = new TextDeletion { Index = 3, Length = 5 };
            TestNodes(mutation, NodeTest2_Input, NodesTest2_Expected);
        }

        [TestMethod]
        public void MutateText_Test()
        {
            var input = new StringBuilder("........123456789.........");
            var mutation = new TextDeletion { Index = 8, Length = 9, };

            mutation.MutateText(input);

            Assert.AreEqual(".................", input.ToString());
        }

        private void TestNodes(Mutation mutation, List<HtmlNode> input, List<HtmlNode> expected)
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
