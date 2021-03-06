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
    public class LowerLetterStringifierTests
    {
        private (int, string)[] _tests = new(int, string)[]
        {
            (1, "a"),
            (2, "b"),
            (3, "c"),
            (27, "aa"),
            (28, "bb"),
            (29, "cc"),
            (79, "aaaa"),
        };

        [TestMethod]
        public void RenderTest()
        {
            var instance = new LowerLetterStringifier();
            foreach (var (input, expected) in _tests)
                Assert.AreEqual(instance.Render(input), expected);
        }
    }
}