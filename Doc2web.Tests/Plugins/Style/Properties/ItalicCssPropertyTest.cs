using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Style.Properties
{
    [TestClass]
    public class ItalicCssPropertyTest
    {
        ItalicCssProperty _instance;

        [TestInitialize]
        public void Initialize()
        {
            _instance = new ItalicCssProperty();
            _instance.Selector = "span.test";
            _instance.Element = new DocumentFormat.OpenXml.Wordprocessing.Italic();
        }

        [TestMethod]
        public void SetOn_Test()
        {
            var data = new CssData();
            var expectedData = new CssData();
            expectedData.AddAttribute("span.test", "font-style", "italic");

            _instance.SetOn(data);

            Assert.AreEqual(expectedData, data);
        }

        [TestMethod]
        public void SetOff_Test()
        {
            var data = new CssData();
            var expectedData = new CssData();
            expectedData.AddAttribute("span.test", "font-style", "normal");

            _instance.SetOff(data);

            Assert.AreEqual(expectedData, data);
        }
    }
}
