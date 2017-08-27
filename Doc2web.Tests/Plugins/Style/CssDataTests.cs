using Doc2web.Plugins.Style;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Style
{
    [TestClass]
    public class CssDataTests
    {
        [TestMethod]
        public void Equals_TrueTest ()
        {
            var cssData1 = new CssData();
            cssData1.AddAttribute("span", "color", "red");

            var cssData2 = new CssData();
            cssData2.AddAttribute("span", "color", "red");

            Assert.AreEqual(cssData1, cssData2);
        }

        [TestMethod]
        public void Equals_FalseTest ()
        {
            var cssData1 = new CssData();
            cssData1.AddAttribute("span", "color", "red");

            var cssData2 = new CssData();
            cssData2.AddAttribute("span", "color", "blue");

            var cssData3 = new CssData();

            var cssData4 = new CssData();
            cssData4.AddAttribute("p", "color", "red");

            Assert.AreNotEqual(cssData1, cssData2);
            Assert.AreNotEqual(cssData1, cssData3);
            Assert.AreNotEqual(cssData1, cssData4);
        }

        [TestMethod]
        public void AddAttribute_NewSelectorTest()
        {
            var cssData = new CssData();
            cssData.AddAttribute("span", "color", "red");

            Assert.AreEqual("red", cssData["span"]["color"]);
        }

        [TestMethod]
        public void AddAttribute_ExisingSelectorTest()
        {
            var cssData = new CssData();
            cssData.AddAttribute("span", "color", "red");
            cssData.AddAttribute("span", "font", "bold");

            Assert.AreEqual("bold", cssData["span"]["font"]);
        }
    }
}
