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
            new HtmlNode{Start = 00, End = 05, Tag = "x", Z = 15 },
            new HtmlNode{Start = 00, End = 05, Tag = "x", Z = 10 },
            new HtmlNode{Start = 00, End = 05, Tag = "x", Z = 05 },
            new HtmlNode{Start = 05, End = 10, Tag = "x", Z = 15 },
            new HtmlNode{Start = 05, End = 10, Tag = "x", Z = 10 },
            new HtmlNode{Start = 05, End = 10, Tag = "x", Z = 05 },
            new HtmlNode{Start = 10, End = 15, Tag = "x", Z = 15 },
            new HtmlNode{Start = 10, End = 15, Tag = "x", Z = 10 },
            new HtmlNode{Start = 10, End = 15, Tag = "x", Z = 05 },
        };

        private static (int, ITag)[] ExpectedZColision => new(int, ITag)[]
        {

           (05, new OpeningTag { Position = Epsilon(00, 0), Name = "x", Z = 15 }),
           (04, new OpeningTag { Position = Epsilon(00, 1), Name = "x", Z = 10 }),
           (03, new OpeningTag { Position = Epsilon(00, 2), Name = "x", Z = 05 }),
           (02, new ClosingTag { Position = Epsilon(05, 0), Z = 05 }),
           (01, new ClosingTag { Position = Epsilon(05, 1), Z = 10 }),
           (00, new ClosingTag { Position = Epsilon(05, 2), Z = 15 }),
           (11, new OpeningTag { Position = Epsilon(05, 3), Name = "x", Z = 15 }),
           (10, new OpeningTag { Position = Epsilon(05, 4), Name = "x", Z = 10 }),
           (09, new OpeningTag { Position = Epsilon(05, 5), Name = "x", Z = 05 }),
           (08, new ClosingTag { Position = Epsilon(10, 0), Z = 05 }),
           (07, new ClosingTag { Position = Epsilon(10, 1), Z = 10 }),
           (06, new ClosingTag { Position = Epsilon(10, 2), Z = 15 }),
           (17, new OpeningTag { Position = Epsilon(10, 3), Name = "x", Z = 15 }),
           (16, new OpeningTag { Position = Epsilon(10, 4), Name = "x", Z = 10 }),
           (15, new OpeningTag { Position = Epsilon(10, 5), Name = "x", Z = 05 }),
           (14, new ClosingTag { Position = Epsilon(15, 0), Z = 05 }),
           (13, new ClosingTag { Position = Epsilon(15, 0), Z = 10 }),
           (12, new ClosingTag { Position = Epsilon(15, 0), Z = 15 }),
        };

        private static List<HtmlNode> RealCase1Input => new List<HtmlNode>
        {
            new HtmlNode { Start = 000, End = 132, Tag = "p", Z = int.MaxValue },
            new HtmlNode { Start = 000, End = 001, Tag = "s", Z = 00 },
            new HtmlNode { Start = 001, End = 009, Tag = "s", Z = 00 },
            new HtmlNode { Start = 009, End = 054, Tag = "s", Z = 00 },
            new HtmlNode { Start = 054, End = 065, Tag = "s", Z = 00 },
            new HtmlNode { Start = 065, End = 132, Tag = "s", Z = 00 },
            new HtmlNode { Start = 001, End = 009, Tag = "s", Z = 05 },
            new HtmlNode { Start = 001, End = 009, Tag = "a", Z = 10 },
            new HtmlNode { Start = 054, End = 065, Tag = "a", Z = 10 },
        };

        private static (int, ITag)[] RealCase1Expected => new(int, ITag)[]
        {
           (17, new OpeningTag { Position = Epsilon(000, 0), Name = "p", Z = int.MaxValue }),
           (02, new OpeningTag { Position = Epsilon(000, 1), Name = "s", Z = 0 }),
           (01, new ClosingTag { Position = Epsilon(001, 0), Z = 0 }),
           (08, new OpeningTag { Position = Epsilon(001, 1), Name = "a", Z = 10 }),
           (07, new OpeningTag { Position = Epsilon(001, 2), Name = "s", Z = 05 }),
           (06, new OpeningTag { Position = Epsilon(001, 3), Name = "s", Z = 00 }),
           (05, new ClosingTag { Position = Epsilon(009, 0), Z = 00 }),
           (04, new ClosingTag { Position = Epsilon(009, 1), Z = 05 }),
           (03, new ClosingTag { Position = Epsilon(009, 2), Z = 10 }),
           (10, new OpeningTag { Position = Epsilon(009, 3), Name = "s", Z = 00 }),
           (09, new ClosingTag { Position = Epsilon(054, 0), Z = 0 }),
           (14, new OpeningTag { Position = Epsilon(054, 1), Name = "a", Z = 10 }),
           (13, new OpeningTag { Position = Epsilon(054, 2), Name = "s", Z = 00 }),
           (12, new ClosingTag { Position = Epsilon(065, 0), Z = 00 }),
           (11, new ClosingTag { Position = Epsilon(065, 1), Z = 10 }),
           (16, new OpeningTag { Position = Epsilon(065, 2), Name = "s", Z = 00 }),
           (15, new ClosingTag { Position = Epsilon(132, 0), Z = 00 }),
           (00, new ClosingTag { Position = Epsilon(132, 1), Z = int.MaxValue }),
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

        [TestMethod]
        public void Sort_RealseCase1Test()
        {
            Test(RealCase1Expected, RealCase1Input);
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
