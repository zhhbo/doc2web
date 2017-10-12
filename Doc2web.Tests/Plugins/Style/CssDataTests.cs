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

            Assert.AreEqual("red", cssData[""]["span"]["color"]);
        }

        [TestMethod]
        public void AddAttribute_ExisingSelectorTest()
        {
            var cssData = new CssData();
            cssData.AddAttribute("span", "color", "red");
            cssData.AddAttribute("span", "font", "bold");

            Assert.AreEqual("bold", cssData[""]["span"]["font"]);
        }

        [TestMethod]
        public void AddAttribute_MediaQueryTest()
        {
            string mq = "(min-width: 30em) and (orientation: landscape)";
            var cssData = new CssData();
            cssData.AddAttribute(mq, "span", "color", "red");
            cssData.AddAttribute(mq, "span", "color", "blue");

            Assert.AreEqual("blue", cssData[mq]["span"]["color"]);
        }

        [TestMethod]
        public void AddScalableAttribte_Test()
        {
            double value = 0.05;
            var cssData = new CssData();
            cssData.AddScalableAttribute(".test", "width", value);

            Assert.AreEqual("5vw", cssData[""][".test"]["width"]);
            Assert.AreEqual("1.08cm", cssData[CssData.FULLPAGE_MEDIAQUERY][".test"]["width"]);
        }

        [TestMethod]
        public void AddRange_Test()
        {
            var expected = new CssData();
            expected.AddAttribute("span", "color", "red");
            expected.AddAttribute("span", "font", "bold");
            expected.AddAttribute("p", "margin-top", "20px");

            var cssData1 = new CssData();
            cssData1.AddAttribute("span", "color", "blue");
            cssData1.AddAttribute("span", "font", "bold");

            var cssData2 = new CssData();
            cssData2.AddAttribute("span", "color", "red");
            cssData2.AddAttribute("p", "margin-top", "20px");

            cssData1.AddRange(cssData2);

            Assert.AreEqual(expected, cssData1);
        }


        [TestMethod]
        public void RenderInto_NoMediaQueryTest()
        {
            string expectedCss = 
                @".test {color: blue;font-weigth: light;}p {color: red;font-weigth: bold;}";
            var cssData = new CssData();
            cssData.AddAttribute("p", "font-weigth", "bold");
            cssData.AddAttribute("p", "color", "red");
            cssData.AddAttribute(".test", "font-weigth", "light");
            cssData.AddAttribute(".test", "color", "blue");

            var sb = new StringBuilder();
            cssData.RenderInto(sb);
            var result = sb.ToString().Trim();

            Assert.AreEqual(expectedCss, result);
        }

        [TestMethod]
        public void RenderInto_MediaQueryTest()
        {
            string mq = "(max-width: 21.59cm)";
            string expectedCss = 
                @"@media (max-width: 21.59cm) { .test {color: blue;font-weigth: light;}p {color: red;font-weigth: bold;} }";
            var cssData = new CssData();
            cssData.AddAttribute(mq, "p", "font-weigth", "bold");
            cssData.AddAttribute(mq, "p", "color", "red");
            cssData.AddAttribute(mq, ".test", "font-weigth", "light");
            cssData.AddAttribute(mq, ".test", "color", "blue");

            var sb = new StringBuilder();
            cssData.RenderInto(sb);
            var result = sb.ToString().Trim();

            Assert.AreEqual(expectedCss, result);
        }
    }
} 