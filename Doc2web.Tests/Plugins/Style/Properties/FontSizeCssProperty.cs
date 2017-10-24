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
            set => _instance.Element = new FontSize { Val = new StringValue(value) };
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
        public void AsCss_UseComplexScriptSizeTest()
        {
            FontSize = "20";
            var set = new CssPropertiesSet
            {
                new FontSizeCSCssProperty(),
                new ComplexScriptCssProperty
                {
                    Element = new DocumentFormat.OpenXml.Wordprocessing.ComplexScript
                    {
                        Val = true
                    }
                }
            };

            var cssData = new CssData();
            _instance.InsertCss(set, cssData);

            Assert.AreEqual(0, cssData.Selectors.Length);
        }
    }
}
