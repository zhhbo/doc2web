﻿using Doc2web.Core.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Tests.Core.Rendering
{
    [TestClass]
    public class TagsSorterTests
    {
        private static List<HtmlNode> InputFlatternColision => new List<HtmlNode>
        {
            new HtmlNode { Start=00, End=10, Tag="x1", Z = 0 },
            new HtmlNode { Start=01, End=10, Tag="y1", Z = 0 },
            new HtmlNode { Start=02, End=10, Tag="z1", Z = 0 },

            new HtmlNode { Start=10, End=10, Tag="x", Z = 0 },
            new HtmlNode { Start=10, End=10, Tag="y", Z = 0 },
            new HtmlNode { Start=10, End=10, Tag="z", Z = 0 },

            new HtmlNode { Start=10, End=22, Tag="x3", Z = 0 },
            new HtmlNode { Start=10, End=21, Tag="y3", Z = 0 },
            new HtmlNode { Start=10, End=20, Tag="z3", Z = 0 },
        };

        private static (int, ITag)[] ExpectedFlatternColision => new(int, ITag)[]
        {
           (05, new OpeningTag { Position = 00, Name = "x1", Z = 0 }),
           (04, new OpeningTag { Position = 01, Name = "y1", Z = 0 }),
           (03, new OpeningTag { Position = 02, Name = "z1", Z = 0 }),

           (02, new ClosingTag { Position = 10 }),
           (01, new ClosingTag { Position = Epsilon(10, 01), Z = 0 }),
           (00, new ClosingTag { Position = Epsilon(10, 02), Z = 0 }),

           (07, new OpeningTag { Position = Epsilon(10, 03), Name = "x", Z = 0 }),
           (06, new ClosingTag { Position = Epsilon(10, 04), Z = 0 }),
           (09, new OpeningTag { Position = Epsilon(10, 05), Name = "y", Z = 0 }),
           (08, new ClosingTag { Position = Epsilon(10, 06), Z = 0 }),
           (11, new OpeningTag { Position = Epsilon(10, 07), Name = "z", Z = 0 }),
           (10, new ClosingTag { Position = Epsilon(10, 08), Z = 0 }),

           (17, new OpeningTag { Position = Epsilon(10, 09), Name = "x3", Z = 0 }),
           (16, new OpeningTag { Position = Epsilon(10, 10), Name = "y3", Z = 0 }),
           (15, new OpeningTag { Position = Epsilon(10, 11), Name = "z3", Z = 0 }),

           (14, new ClosingTag { Position = 20, Z = 0 }),
           (13, new ClosingTag { Position = 21, Z = 0 }),
           (12, new ClosingTag { Position = 22, Z = 0 }),
        };

        private static List<HtmlNode> InputZColision => new List<HtmlNode>
        {
            new HtmlNode{Start = 0, End = 10, Tag = "div",  Z = 15 },
            new HtmlNode{Start = 0, End = 10, Tag = "a",    Z = 10 },
            new HtmlNode{Start = 0, End = 10, Tag = "span", Z = 05 },
        };

        private static (int, ITag)[] ExpectedZColision => new(int, ITag)[]
        {
           (05, new OpeningTag { Position = 00, Name = "div", Z = 15 }),
           (04, new OpeningTag { Position = Epsilon(00, 1), Name = "a", Z = 10 }),
           (03, new OpeningTag { Position = Epsilon(00, 2), Name = "span", Z = 05 }),

           (02, new ClosingTag { Position = 10, Z = 05 }),
           (01, new ClosingTag { Position = Epsilon(10, 01), Z = 10 }),
           (00, new ClosingTag { Position = Epsilon(10, 02), Z = 15 }),
        };


        private static double Epsilon(double v, int eCount) =>
            v + double.Epsilon * eCount;

        [TestMethod]
        public void Sort_FlatternTest()
        {
            Test(ExpectedFlatternColision, InputFlatternColision);
        }

        [TestMethod]
        public void Sort_ZTest()
        {
            Test(ExpectedZColision, InputZColision);
        }

        private void Test((int, ITag)[] expectedConfig, List<HtmlNode> sample)
        {
            ITag[] expected = Utils.SetRelatedTag(expectedConfig);
            var result = TagsFactory.Build(sample.ToArray());
            var random = new Random();

            for (int i = 0; i < 100; i++)
            {
                var unsorted = result.OrderBy(x => random.NextDouble()).ToArray();
                var sorted = TagsSorter.Sort(unsorted);

                Utils.AssertTagsArraysAreEquals(expected, sorted);
            }
        }
    }
}
