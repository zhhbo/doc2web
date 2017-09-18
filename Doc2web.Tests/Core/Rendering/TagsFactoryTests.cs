using Doc2web.Core.Rendering;
using Doc2web.Core.Rendering.Step2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Tests.Core.Rendering.Step2
{
    [TestClass]
    public class TagsFactoryTests
    {
        private static List<HtmlNode> InputOpenClose => new List<HtmlNode>
        {
            new HtmlNode { Start=0, End=10, Z=0, Tag="div" }
        };

        private static (int, ITag)[] ExpectedOpenClose => new(int, ITag)[]
        {
           (1, new OpeningTag { Position = 0, Name = "div" }),
           (0, new ClosingTag { Position = 10 })
        };


        private static List<HtmlNode> InputSelfClosing => new List<HtmlNode>
        {
            new HtmlNode { Start = 0, End = 0, Z = 0, Tag = "br" }
        };

        private static (int, ITag)[] ExpectedSelfClosing => new(int, ITag)[]
        {
           (0, new SelfClosingTag { Position = 0, Name = "br" }),
        };

        [TestMethod]
        public void Build_OpenCloseTest()
        {
            Test(ExpectedOpenClose, InputOpenClose);
        }

        [TestMethod]
        public void Build_SelfClosingTest()
        {
            Test(ExpectedSelfClosing, InputSelfClosing);
        }


        private void Test((int, ITag)[] expectedConfig, List<HtmlNode> sample, double minLimit = 0, double maxLimit = double.MaxValue)
        {
            ITag[] expected = Utils.SetRelatedTag(expectedConfig);
            var result = TagsFactory.Build(sample.ToArray());
            Assert.AreEqual(expected.Length, result.Length);

            for (int i = 0; i < result.Length; i++)
            {
                var r = result[i];
                var e = expected[i];

                AssertTagsAreEquals(e, r);
            }

        }

        private void AssertTagsAreEquals(ITag e, ITag r)
        {
            switch (e)
            {
                case SelfClosingTag selfClosingE when r is SelfClosingTag:
                    AssertSelfClosingTagAreEqual(selfClosingE, (SelfClosingTag)r);
                    break;

                case OpeningTag openingE when r is OpeningTag:
                    AssertOpeningTagsAreEqual(openingE, (OpeningTag)r);
                    break;

                case ClosingTag closingE when r is ClosingTag:
                    AssertClosingTagsAreEqual(closingE, (ClosingTag)r);
                    break;

                default:
                    Assert.Fail("Tags are not the same type");
                    break;
            }
        }

        private void AssertSelfClosingTagAreEqual(SelfClosingTag expected, SelfClosingTag result)
        {
            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.Position, result.Position);
            AssertAttributesAreEquals(expected.Attributes, result.Attributes);
        }

        private void AssertOpeningTagsAreEqual(OpeningTag expected, OpeningTag result)
        {
            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.Position, result.Position);
            Assert.AreEqual(expected.Attributes, result.Attributes);
            AssertAttributesAreEquals(expected.Attributes, result.Attributes);
            AssertClosingTagsAreEqual(expected.Related, result.Related);
        }

        private void AssertClosingTagsAreEqual(ClosingTag expected, ClosingTag result)
        {
            Assert.AreEqual(expected.Position, result.Position);
            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.RelatedPosition, result.RelatedPosition);
        }

        private void AssertAttributesAreEquals
            (IReadOnlyDictionary<string, string> expected, IReadOnlyDictionary<string, string> result)
        {
            Assert.AreEqual(expected.Keys.ToArray(), result.Keys.ToArray());
            Assert.AreEqual(expected.Values.ToArray(), result.Values.ToArray());
        }
    }
}
