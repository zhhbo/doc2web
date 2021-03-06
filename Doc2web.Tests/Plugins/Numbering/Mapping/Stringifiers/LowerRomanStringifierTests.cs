﻿using Doc2web.Plugins.Numbering.Mapping.Stringifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Tests.Plugins.Numbering.Mapping.Stringifiers
{
    [TestClass]
    public class LowerRomanStringifierTests
    {
        private (int, string)[] _tests = new(int, string)[]
        {
            (1, "i"),
            (2, "ii"),
            (3, "iii"),
            (4, "iv"),
            (5, "v"),
            (6, "vi"),
            (7, "vii"),
            (8, "viii"),
            (9, "ix"),
            (10, "x"),
            (11, "xi"),
            (12, "xii"),
            (13, "xiii"),
            (14, "xiv"),
            (45, "xlv"),
            (56, "lvi"),
            (99, "xcix"),
            (222, "ccxxii"),
            (401, "cdi"),
            (501, "di"),
            (999, "cmxcix"),
        };

        [TestMethod]
        public void Render_Test()
        {
            var instance = new LowerRomanStringifier();
            foreach (var (input, expected) in _tests)
                Assert.AreEqual(instance.Render(input), expected);
        }

    }
}