using Doc2web.Core.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Tests.Core.Rendering
{
    [TestClass]
    public class TagFactoryTests
    {
        private static List<HtmlNode> InputOpenClose =>
            new List<HtmlNode>
            {
                new HtmlNode { Start=0, End=10, Z=0, Tag="div" }
            };

        private static ITag[] ExpectedOpenClose
        {
            get
            {
                var tagOpening = new OpeningTag { Index = 0, Z = 0, Name = "div" };
                var tagClosing = new ClosingTag { Related = tagOpening, Index = 10 };
                return new ITag[] { tagOpening, tagClosing };
            }
        }


        private static List<HtmlNode> InputSelfClosing =>
            new List<HtmlNode>
            {
                new HtmlNode { Start = 0, End = 0, Z = 0, Tag = "br" }
            };

        private static ITag[] ExpectedSelfClosing
        {
            get =>
                 new ITag[] {
                     new SelfClosingTag { Name = "br", Index=0, Z = 0}
                 };
        }

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

        private void Test(ITag[] expected, List<HtmlNode> sample)
        {
            var result = TagFactory.Build(sample);
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
            Assert.AreEqual(expected.Index, result.Index);
            AssertAttributesAreEquals(expected.Attributes, result.Attributes);
        }

        private void AssertOpeningTagsAreEqual(OpeningTag expected, OpeningTag result)
        {
            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.Index, result.Index);
            Assert.AreEqual(expected.Attributes, result.Attributes);
            Assert.AreEqual(expected.Z, result.Z);
            AssertAttributesAreEquals(expected.Attributes, result.Attributes);
            AssertClosingTagsAreEqual(expected.Related, result.Related);
        }

        private void AssertClosingTagsAreEqual(ClosingTag expected, ClosingTag result)
        {
            Assert.AreEqual(expected.Index, result.Index);
            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.RelatedIndex, result.RelatedIndex);
            Assert.AreEqual(expected.Z, result.Z);
        }

        private void AssertAttributesAreEquals
            (IReadOnlyDictionary<string, string> expected, IReadOnlyDictionary<string, string> result)
        {
            Assert.AreEqual(expected.Keys.ToArray(), result.Keys.ToArray());
            Assert.AreEqual(expected.Values.ToArray(), result.Values.ToArray());
        }
    }
}
