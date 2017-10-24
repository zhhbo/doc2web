using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Style.Properties
{
    [TestClass]
    public class FontSizeCSCssPropertyTests
    {
        private FontSizeCSCssProperty _instance;
        private CssPropertiesSet _set;

        [TestInitialize]
        public void Initialize()
        {
            _set = new CssPropertiesSet();
            _instance = new FontSizeCSCssProperty
            {
                Selector = ".test",
                Element = new DocumentFormat.OpenXml.Wordprocessing.FontSizeComplexScript
                {
                    Val = "10"
                }
            };
        }

        [TestMethod]
        public void InsertCss_ComplexScriptPropTest()
        {
            _set = new CssPropertiesSet
            {
                new ComplexScriptCssProperty
                {
                    Element = new DocumentFormat.OpenXml.Wordprocessing.ComplexScript
                    {
                        Val = true
                    }
                }
            };

            AssertCssInserted();
        }

        [TestMethod]
        public void InsertCss_OnlyComplexScriptFontTest()
        {
            _set = new CssPropertiesSet
            {
                new RunFontsCssProperty(null)
                {
                    Element = new DocumentFormat.OpenXml.Wordprocessing.RunFonts
                    {
                        ComplexScript = "arial"
                    }
                }
            };

            AssertCssInserted();
        }

        [TestMethod]
        public void InsertCss_NotRequiredTest()
        {
            AssertCssNotInserted();
        }

        private void AssertCssNotInserted()
        {
            var cssData = new CssData();
            _instance.InsertCss(_set, cssData);

            Assert.AreEqual(0, cssData.Selectors.Length);
        }

        private void AssertCssInserted()
        {
            var cssData = new CssData();
            _instance.InsertCss(_set, cssData);

            Assert.IsTrue(cssData[""][".test"].ContainsKey("font-size"));
            Assert.AreEqual("5pt", cssData[""][".test"]["font-size"]);
        }

    }
}
