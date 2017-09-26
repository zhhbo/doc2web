using Doc2web.Plugins.Numbering.Mapping.Stringifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Tests.Plugins.Numbering.Mapping.Stringifiers
{
    [TestClass]
    public class OrdinalNumberFormatRendererTests
    {

        private (int, string)[] _tests = new(int, string)[]
        {
            (1, "1st"),
            (2, "2nd"),
            (3, "3rd"),
            (4, "4th"),
            (9, "9th"),
            (10, "10th"),
            (12, "12th"),
            (21, "21st"),
            (92, "92nd"),
            (121, "121st"),
        };

        [TestMethod]
        public void RenderTest()
        {
            var instance = new OrdinalNumberStringifier();
            foreach (var (input, expected) in _tests)
                Assert.AreEqual(instance.Render(input), expected);
        }
    }
}