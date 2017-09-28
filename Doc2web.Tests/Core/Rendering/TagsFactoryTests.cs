using Doc2web.Core.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Tests.Core.Rendering
{
    [TestClass]
    public class TagsFactoryTests
    {
        private static List<HtmlNode> InputOpenClose => new List<HtmlNode>
        {
            new HtmlNode { Start=0, End=10, Z=0, Tag="div", TextPrefix="prefix", TextSuffix="suffix" }
        };

        private static (int, ITag)[] ExpectedOpenClose => new(int, ITag)[]
        {
           (1, new OpeningTag { Position = 0, Name = "div", TextAfter="prefix" }),
           (0, new ClosingTag { Position = 10, TextBefore="suffix" })
        };


        private static List<HtmlNode> InputSelfClosing => new List<HtmlNode>
        {
            new HtmlNode { Start = 0, End = 0, Z = 0, Tag = "br" }
        };

        private static (int, ITag)[] ExpectedSelfClosing => new(int, ITag)[]
        {
           (0, new SelfClosingTag { Position = 0, Name = "br" }),
        };

        private static List<HtmlNode> InputFlatternColision => new List<HtmlNode>
        {
            new HtmlNode { Start=00, End=10, Tag="x1" },
            new HtmlNode { Start=01, End=10, Tag="y1" },
            new HtmlNode { Start=02, End=10, Tag="z1" },

            new HtmlNode { Start=10, End=10, Tag="x2" },
            new HtmlNode { Start=10, End=10, Tag="y2" },
            new HtmlNode { Start=10, End=10, Tag="z2" },

            new HtmlNode { Start=10, End=22, Tag="x3" },
            new HtmlNode { Start=10, End=21, Tag="y3" },
            new HtmlNode { Start=10, End=20, Tag="z3" },
        };

        private static (int, ITag)[] ExpectedFlatternColision => new(int, ITag)[]
        {
           (05, new OpeningTag { Position = 00, Name = "x1" }),
           (04, new OpeningTag { Position = 01, Name = "y1" }),
           (03, new OpeningTag { Position = 02, Name = "z1" }),

           (02, new ClosingTag { Position = 10 }),
           (01, new ClosingTag { Position = 10 }),
           (00, new ClosingTag { Position = 10 }),

           (07, new OpeningTag { Position = 10, Name = "x2" }),
           (06, new ClosingTag { Position = 10 }),
           (09, new OpeningTag { Position = 10, Name = "y2" }),
           (08, new ClosingTag { Position = 10 }),
           (11, new OpeningTag { Position = 10, Name = "z2" }),
           (10, new ClosingTag { Position = 10 }),

           (17, new OpeningTag { Position = 10, Name = "x3" }),
           (16, new OpeningTag { Position = 10, Name = "y3" }),
           (15, new OpeningTag { Position = 10, Name = "z3" }),

           (14, new ClosingTag { Position = 20 }),
           (13, new ClosingTag { Position = 21 }),
           (12, new ClosingTag { Position = 22 }),
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

        [TestMethod]
        public void Build_FlatternColisionTest()
        {
            Test(ExpectedFlatternColision, InputFlatternColision);
        }


        private void Test((int, ITag)[] expectedConfig, List<HtmlNode> sample)
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
            Assert.AreEqual(expected.TextAfter, result.TextAfter);
            AssertAttributesAreEquals(expected.Attributes, result.Attributes);
            AssertClosingTagsAreEqual(expected.Related, result.Related);
        }

        private void AssertClosingTagsAreEqual(ClosingTag expected, ClosingTag result)
        {
            Assert.AreEqual(expected.Position, result.Position);
            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.RelatedPosition, result.RelatedPosition);
            Assert.AreEqual(expected.TextBefore, result.TextBefore);
        }

        private void AssertAttributesAreEquals
            (IReadOnlyDictionary<string, string> expected, IReadOnlyDictionary<string, string> result)
        {
            Assert.AreEqual(expected.Keys.ToArray(), result.Keys.ToArray());
            Assert.AreEqual(expected.Values.ToArray(), result.Values.ToArray());
        }
    }
}
