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

        private void Test((int, ITag)[] expectedConfig, List<HtmlNode> sample)
        {
            ITag[] expected = Utils.SetRelatedTag(expectedConfig);
            var result = TagsFactory.Build(sample.ToArray());
            Utils.AssertTagsArraysAreEquals(expected, result);
        }

        
    }
}
