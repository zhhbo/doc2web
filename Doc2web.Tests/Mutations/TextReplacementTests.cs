using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Tests.Mutations
{
    [TestClass]
    public class TextReplacementTests
    {

        [TestMethod]
        public void TextReplacement_RenderTest()
        {
            var instance = new TextReplacement
            {
                Replacement = "Some content"
            };
            Assert.AreEqual(instance.Replacement, instance.Render());
        }

        [TestMethod]
        public void TextReplacement_ZeroOffsetTest()
        {
            var instance = new TextReplacement
            {
                Length = 1,
                Replacement = "1234"
            };
            Assert.AreEqual(0, instance.Offset);
        }

        [TestMethod]
        public void TextReplacement_PositiveOffsetTest()
        {
            var instance = new TextReplacement
            {
                Length = 4,
                Replacement = "1"
            };
            Assert.AreEqual(3, instance.Offset);
        }
    }
}
