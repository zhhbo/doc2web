using Doc2web.Core.Rendering;
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
            new HtmlNode { Start=00, End=10, Tag="x1" },
            new HtmlNode { Start=01, End=10, Tag="y1" },
            new HtmlNode { Start=02, End=10, Tag="z1" },

            new HtmlNode { Start=10, End=10, Tag="x" },
            new HtmlNode { Start=10, End=10, Tag="y" },
            new HtmlNode { Start=10, End=10, Tag="z" },

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
           (01, new ClosingTag { Position = Epsilon(10, 01) }),
           (00, new ClosingTag { Position = Epsilon(10, 02) }),

           (07, new OpeningTag { Position = Epsilon(10, 03), Name = "x" }),
           (06, new ClosingTag { Position = Epsilon(10, 04) }),
           (09, new OpeningTag { Position = Epsilon(10, 05), Name = "y" }),
           (08, new ClosingTag { Position = Epsilon(10, 06) }),
           (11, new OpeningTag { Position = Epsilon(10, 07), Name = "z" }),
           (10, new ClosingTag { Position = Epsilon(10, 08) }),

           (17, new OpeningTag { Position = Epsilon(10, 09), Name = "x3" }),
           (16, new OpeningTag { Position = Epsilon(10, 10), Name = "y3" }),
           (15, new OpeningTag { Position = Epsilon(10, 11), Name = "z3" }),

           (14, new ClosingTag { Position = 20 }),
           (13, new ClosingTag { Position = 21 }),
           (12, new ClosingTag { Position = 22 }),
        };

        private static double Epsilon(double v, int eCount) =>
            v + double.Epsilon * eCount;

        [TestMethod]
        public void Sort_Test()
        {
            Test(ExpectedFlatternColision, InputFlatternColision);
        }
        
        private void Test((int, ITag)[] expectedConfig, List<HtmlNode> sample)
        {
            ITag[] expected = Utils.SetRelatedTag(expectedConfig);
            var result = TagsFactory.Build(sample.ToArray());
            var random = new Random();

            for(int i = 0; i < 100; i++)
            {
                var unsorted = result.OrderBy(x => random.NextDouble()).ToArray();
                var sorted = TagsSorter.Sort(unsorted);

                Utils.AssertTagsArraysAreEquals(expected, sorted);
            }
        }
    }
}
