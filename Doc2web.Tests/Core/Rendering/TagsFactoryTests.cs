﻿using Doc2web.Core.Rendering;
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
           (1, new OpeningTag { Index = 0, Z = 0, Name = "div" }),
           (0, new ClosingTag { Index = 10 })
        };


        private static List<HtmlNode> InputSelfClosing => new List<HtmlNode>
        {
            new HtmlNode { Start = 0, End = 0, Z = 0, Tag = "br" }
        };

        private static (int, ITag)[] ExpectedSelfClosing => new(int, ITag)[]
        {
           (0, new SelfClosingTag { Index = 0, Z = 0, Name = "br" }),
        };

        private static List<HtmlNode> InputSorting1 => new List<HtmlNode>
        {
            new HtmlNode { Start = 00, End = 10, Z = 100 },
            new HtmlNode { Start = 00, End = 05, Z = 050 },
            new HtmlNode { Start = 05, End = 10, Z = 050 }
        };

        private static (int, ITag)[] ExpectedSorting1 => new(int, ITag)[]
        {
           (5, new OpeningTag { Index = 00, Z = 100 }),
           (2, new OpeningTag { Index = 00, Z = 050 }),
           (1, new ClosingTag { Index = 05 }),
           (4, new OpeningTag { Index = 05, Z = 050 }),
           (3, new ClosingTag { Index = 10 }),
           (0, new ClosingTag { Index = 10 })
        };

        private static List<HtmlNode> InputSorting2 => new List<HtmlNode>
        {
            new HtmlNode { Start = 00, End = 05, Z = 100, Tag="div" },
            new HtmlNode { Start = 00, End = 05, Z = 050, Tag="span" },
            new HtmlNode { Start = 05, End = 10, Z = 050, Tag="span" },
            new HtmlNode { Start = 05, End = 10, Z = 100, Tag="div" },
        };

        private static (int, ITag)[] ExpectedSorting2 => new(int, ITag)[]
        {
           (3, new OpeningTag { Index = 00, Z = 100, Name="div" }),
           (2, new OpeningTag { Index = 00, Z = 050, Name="span" }),
           (1, new ClosingTag { Index = 05 }),
           (0, new ClosingTag { Index = 05 }),
           (7, new OpeningTag { Index = 05, Z = 100, Name="div" }),
           (6, new OpeningTag { Index = 05, Z = 050, Name="span" }),
           (5, new ClosingTag { Index = 10 }),
           (4, new ClosingTag { Index = 10 })
        };

        private static List<HtmlNode> InputSorting3 => new List<HtmlNode>
        {
            new HtmlNode { Start = 00, End = 10, Z = 075, Tag="div" },
            new HtmlNode { Start = 00, End = 00, Z = 100, Tag="hr" },
            new HtmlNode { Start = 00, End = 00, Z = 050, Tag="hr" },
            new HtmlNode { Start = 05, End = 05, Z = 050, Tag="br" },
            new HtmlNode { Start = 05, End = 05, Z = 100, Tag="br" },
            new HtmlNode { Start = 10, End = 10, Z = 050, Tag="hr" },
            new HtmlNode { Start = 10, End = 10, Z = 100, Tag="hr" },
        };

        private static (int, ITag)[] ExpectedSorting3 => new(int, ITag)[]
        {
           (0, new SelfClosingTag { Index = 00, Z = 100, Name = "hr" }),
           (6, new OpeningTag     { Index = 00, Z = 075, Name = "div" }),
           (2, new SelfClosingTag { Index = 00, Z = 050, Name = "hr" }),
           (3, new SelfClosingTag { Index = 05, Z = 050, Name = "br" }),
           (4, new SelfClosingTag { Index = 05, Z = 100, Name = "br" }),
           (3, new SelfClosingTag { Index = 10, Z = 050, Name = "hr" }),
           (1, new ClosingTag     { Index = 10 }),
           (5, new SelfClosingTag { Index = 10, Z = 100, Name = "hr" }),
        };

        private static List<HtmlNode> InputSorting4 => new List<HtmlNode>
        {
            new HtmlNode { Start = 0, End = 0, Z = 0, Tag="a" },
            new HtmlNode { Start = 0, End = 0, Z = 0, Tag="b" },
            new HtmlNode { Start = 0, End = 1, Z = 0, Tag="w" },
            new HtmlNode { Start = 0, End = 1, Z = 0, Tag="x" },
            new HtmlNode { Start = 0, End = 1, Z = 0, Tag="y" },
            new HtmlNode { Start = 1, End = 1, Z = 0, Tag="z" },
        };

        private static (int, ITag)[] ExpectedSorting4 => new(int, ITag)[]
        {
           (0, new SelfClosingTag { Index = 0, Z = 0, Name = "a" }),
           (1, new SelfClosingTag { Index = 0, Z = 0, Name = "b" }),
           (7, new OpeningTag     { Index = 0, Z = 0, Name = "w" }),
           (6, new OpeningTag     { Index = 0, Z = 0, Name = "x" }),
           (5, new OpeningTag     { Index = 0, Z = 0, Name = "y" }),
           (4, new ClosingTag     { Index = 1 }),
           (3, new ClosingTag     { Index = 1 }),
           (2, new ClosingTag     { Index = 1 }),
           (8, new SelfClosingTag { Index = 1, Z = 100, Name = "z" }),
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
        public void Build_SortingTest1()
        {
            Test(ExpectedSorting1, InputSorting1);
        }

        [TestMethod]
        public void Build_SortingTest2()
        {
            Test(ExpectedSorting2, InputSorting2);
        }

        [TestMethod]
        public void Build_SortingTest3()
        {
            Test(ExpectedSorting3, InputSorting3);
        }

        [TestMethod]
        public void Build_SortingTest4()
        {
            var random = new Random();
            var expected = ExpectedSorting4;
            for(int i=0; i<100; i++)
            {
                var shuffledInput = InputSorting4.OrderBy(x => random.Next()).ToList();
                Test(expected, shuffledInput);
            }
        }

        private void Test((int, ITag)[] expectedConfig, List<HtmlNode> sample)
        {
            ITag[] expected = Utils.SetRelatedTag(expectedConfig);
            var result = TagsFactory.Build(sample);
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