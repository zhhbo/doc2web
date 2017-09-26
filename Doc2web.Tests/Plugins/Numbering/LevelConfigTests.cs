using Doc2web.Plugins.Numbering;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Tests.Plugins.Numbering
{
    [TestClass]
    public class LevelConfigTests
    {
        [TestMethod]
        public void RenderNumberTest()
        {
            var instance = new LevelConfig
            {
                NumberingFormat = NumberFormatValues.Decimal
            };
            Assert.AreEqual("1", instance.RenderNumber(1));

            instance.NumberingFormat = NumberFormatValues.Ordinal;
            Assert.AreEqual("1st", instance.RenderNumber(1));

            instance.NumberingFormat = NumberFormatValues.OrdinalText;
            Assert.AreEqual("one", instance.RenderNumber(1));

            instance.NumberingFormat = NumberFormatValues.LowerRoman;
            Assert.AreEqual("i", instance.RenderNumber(1));

            instance.NumberingFormat = NumberFormatValues.UpperRoman;
            Assert.AreEqual("I", instance.RenderNumber(1));

            instance.NumberingFormat = NumberFormatValues.LowerLetter;
            Assert.AreEqual("a", instance.RenderNumber(1));

            instance.NumberingFormat = NumberFormatValues.UpperLetter;
            Assert.AreEqual("A", instance.RenderNumber(1));
        }
    }
}