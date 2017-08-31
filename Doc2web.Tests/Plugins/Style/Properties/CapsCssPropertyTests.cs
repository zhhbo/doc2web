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
            _instance = new CapsCssProperty();
            _instance.Selector = "span.test-cls";
        }

        [TestMethod]
        public void AssCss_OnTest()
        {
            var expectedCssData = new CssData();
            expectedCssData.AddAttribute("span.test-cls", "text-transform", "uppercase");

            var cssData = new CssData();
            _instance.SetOn(cssData);

            Assert.AreEqual(expectedCssData, cssData);
        }

        [TestMethod]
        public void AssCss_OffTest()
        {
            var expectedCssData = new CssData();
            expectedCssData.AddAttribute("span.test-cls", "text-transform", "lowercase");

            var cssData = new CssData();
            _instance.SetOff(cssData);

            Assert.AreEqual(expectedCssData, cssData);
        }
    }
}
