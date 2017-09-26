using Doc2web.Plugins.Numbering.Stringifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Tests.Plugins.Numbering.Stringifiers
{
    [TestClass]
    public class OrdinalTextNumberFormatRendrerTests
    {
        private (int, string)[] _tests = new(int, string)[]
        {
            (1, "one"),
            (2, "two"),
            (3, "three"),
            (4, "four"),
            (5, "five"),
            (6, "six"),
            (7, "seven"),
            (8, "eight"),
            (9, "nine"),
            (10, "ten"),
            (24, "twenty-four"),
            (101, "one hundred and one"),
        };

        [TestMethod]
        public void RenderLowerTest()
        {
            var instance = new OrdinalTextStringifier();
            foreach (var (input, expected) in _tests)
                Assert.AreEqual(instance.Render(input), expected);
        }
    }
}