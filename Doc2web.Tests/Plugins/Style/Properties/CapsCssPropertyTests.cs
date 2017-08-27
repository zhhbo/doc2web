using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Properties;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Style.Properties
{
    [TestClass]
    public class CapsCssPropertyTests
    {
        private CapsCssProperty _instance;

        [TestInitialize]
        public void Initialize()
        {
            _instance = new CapsCssProperty { Element = new Caps() };
            _instance.Selector = "span.test-cls";
        }


        [TestMethod]
        public void AssCss_DefaultTest()
        {
            var expectedCssData = new CssData();
            expectedCssData.AddAttribute("span.test-cls", "text-transform", "uppercase");

            var cssData = _instance.AsCss();

            Assert.AreEqual(expectedCssData, cssData);
        }

        [TestMethod]
        public void AssCss_CapsTest()
        {
            _instance.Element.Val = new DocumentFormat.OpenXml.OnOffValue(true);
            var expectedCssData = new CssData();
            expectedCssData.AddAttribute("span.test-cls", "text-transform", "uppercase");

            var cssData = _instance.AsCss();

            Assert.AreEqual(expectedCssData, cssData);
        }

        [TestMethod]
        public void AssCss_NoCapsTest()
        {
            _instance.Element.Val = new DocumentFormat.OpenXml.OnOffValue(false);
            var expectedCssData = new CssData();
            expectedCssData.AddAttribute("span.test-cls", "text-transform", "lowercase");

            var cssData = _instance.AsCss();

            Assert.AreEqual(expectedCssData, cssData);

        }

    }
}
