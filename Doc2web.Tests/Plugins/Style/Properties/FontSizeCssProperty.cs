using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Doc2web.Plugins.Style.Properties;
using DocumentFormat.OpenXml;
using Doc2web.Plugins.Style;

namespace Doc2web.Tests.Plugins.Style.Properties
{
    [TestClass]
    public class FontSizeCssPropertyTests
    {
        private FontSizeCssProperty _instance;

        public string FontSize
        {
            get => _instance.Element.Val.Value;
            set => _instance.Element.Val = new StringValue(value);
        }

        [TestInitialize]
        public void Initialize()
        {
            _instance = new FontSizeCssProperty
            {
                Selector = "span.test",
                Element = new FontSize()
            };
        }

        [TestMethod]
        public void AsCss_Test()
        {
            var expectedCss = new CssData();
            expectedCss.AddAttribute("span.test", "font-size", "10pt");
            FontSize = "20";

            var data = _instance.AsCss();

            Assert.AreEqual(expectedCss, data);
        }

        [TestMethod]
        public void GetSpecificHashcode_Test()
        {
            FontSize = "35";

            Assert.AreEqual(35, _instance.GetSpecificHashcode());
        }

        [TestMethod]
        public void HaveSameOutput_TrueTest()
        {
            var element = new FontSize { Val = new StringValue("40") };
            FontSize = "40";

            Assert.IsTrue(_instance.HaveSameOuput(element));
        }

        [TestMethod]
        public void HaveSameOutput_FalseTest()
        {
            var element = new FontSize { Val = new StringValue("40") };
            FontSize = "30";

            Assert.IsFalse(_instance.HaveSameOuput(element));
        }
    }
}
