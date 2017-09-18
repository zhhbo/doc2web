using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Tests.Mutations
{
    [TestClass]
    public class TextReplacementTests
    {
        private StringBuilder _text;
        private TextReplacement _instance;
        private List<HtmlNode> _nodes;
        private List<Mutation> _otherMutations;

        [TestInitialize]
        public void Initialize()
        {
            _text = new StringBuilder("This is a Complete test.");
            _instance = new TextReplacement()
            {
                Index = _text.ToString().IndexOf("Complete"),
                Length = "Complete".Length
            };

            _nodes = new List<HtmlNode>
            {
                new HtmlNode { Start = 0, End = 3 },
                new HtmlNode { Start = _instance.Index + 5, End = _instance.Index + 6 },
                new HtmlNode { Start = 19, End = 20 }
            };

            _otherMutations = new List<Mutation>
            {
                Substitute.For<Mutation>(),
                Substitute.For<Mutation>(),
            };
            _otherMutations[0].Index = _instance.Index + 3;
            _otherMutations[1].Index = _instance.Index + 9;
        }

        [TestMethod]
        public void UpdateOtherMutations_SmallerText()
        {
            var replacement = "s";
            var offset = replacement.Length - _instance.Length;
            TestMutationUpdate(
                replacement,
                null,
                _otherMutations[1].Index + offset);
        }

        [TestMethod]
        public void UpdateOtherMutations_BiggerText()
        {
            var replacement = "BiggerThanTheOther";
            var offset = replacement.Length - _instance.Length;
            TestMutationUpdate(
                replacement,
                _otherMutations[0].Index,
                _otherMutations[1].Index + offset);
        }

        [TestMethod]
        public void UpdateOtherMutations_EqText()
        {
            var replacement = "etelpmoC";
            TestMutationUpdate(
                replacement,
                _otherMutations[0].Index,
                _otherMutations[1].Index);
        }

        [TestMethod]
        public void MutateNodes_SmallerTest()
        {
            var replacement = "123";
            var delta = replacement.Length - _instance.Length;
            var expected = new List<HtmlNode>
            {
                new HtmlNode { Start = 0, End = 3 },
                new HtmlNode { Start = _nodes[2].Start + delta, End = _nodes[2].End + delta },
            };

            TestNodeMutations(replacement, expected);
        }

        [TestMethod]
        public void MutateNodes_BiggerTest()
        {
            var replacement = "ThisIsWayBigger";
            var delta = replacement.Length - _instance.Length;
            var expected = new List<HtmlNode>
            {
                new HtmlNode { Start = 0, End = 3 },
                new HtmlNode { Start = _nodes[1].Start, End = _nodes[1].End },
                new HtmlNode { Start = _nodes[2].Start + delta, End = _nodes[2].End + delta },
            };

            TestNodeMutations(replacement, expected);
        }

        [TestMethod]
        public void MutateNodes_EqTest()
        {
            TestNodeMutations("Complete", new List<HtmlNode>(_nodes));
        }

        [TestMethod]
        public void MutateText_SmallerTest()
        {
            TestTextMutation("smaller");
        }

        [TestMethod]
        public void MutateText_BiggerTest()
        {
            TestTextMutation("BiggerThanTheOther");
        }

        [TestMethod]
        public void MutateText_EqTest()
        {
            TestTextMutation("etelpmoC");
        }

        private void TestMutationUpdate(string replacement, double? m1, double? m2)
        {
            _instance.Replacement = replacement;
            var result = new List<Mutation>(_otherMutations);
            _instance.UpdateOtherMutations(result);

            if (m1.HasValue)
            {
                Assert.IsTrue(result.Contains(_otherMutations[0]));
                Assert.AreEqual(m1.Value, _otherMutations[0].Index);
            } else
                Assert.IsFalse(result.Contains(_otherMutations[0]));

            if (m2.HasValue)
            {
                Assert.IsTrue(result.Contains(_otherMutations[1]));
                Assert.AreEqual(m2.Value, _otherMutations[1].Index);
            } else
                Assert.IsFalse(result.Contains(_otherMutations[1]));
        }

        private void TestNodeMutations(string replacement, List<HtmlNode> expected)
        {
            _instance.Replacement = replacement;
            _instance.MutateNodes(_nodes);
            Assert.AreEqual(expected.Count, _nodes.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Start, _nodes[i].Start);
                Assert.AreEqual(expected[i].End, _nodes[i].End);
            }
        }

        private void TestTextMutation(string replacement)
        {
            string expected = _text.ToString().Replace("Complete", replacement);
            _instance.Replacement = replacement;
            _instance.MutateText(_text);
            Assert.AreEqual(expected, _text.ToString());
        }
    }
}
