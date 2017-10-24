using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Properties;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Style.Properties
{
    [TestClass]
    public class HighlightCssPropertyTests
    {
        private HighlightCssProperty _instance;

        public HighlightColorValues HighlightColor
        {
            set
            {
                _instance.Element.Val = new EnumValue<HighlightColorValues>(value);
            }
            get => _instance.Element.Val.Value;
        }

        [TestInitialize]
        public void Initialize()
        {
            _instance = new HighlightCssProperty
            {
                Selector = "span.test",
                Element = new Highlight()
            };
        }

        [TestMethod]
        public void AsCss_Test()
        {
            var expectedCss = new CssData();
            expectedCss.AddAttribute("span.test", "background-color", "#8B8B00");
            HighlightColor = HighlightColorValues.DarkYellow;

            var data = _instance.AsCss();

            Assert.AreEqual(expectedCss, data);
        }

        [TestMethod]
        public void GetHashCode_ColorTest ()
        {
            HighlightColor = HighlightColorValues.Black;

            Assert.AreEqual(0, _instance.GetHashCode());
        }

        [TestMethod]
        public void GetHashCode_NotColorTest()
        {
            Assert.AreEqual(-1, _instance.GetHashCode());
        }

        [TestMethod]
        public void HasSameOuput_TrueTest()
        {
            var highlightColor = HighlightColorValues.DarkGray;
            _instance.Element.Val = highlightColor;
            var elem = new Highlight
            {
                Val = new EnumValue<HighlightColorValues>(highlightColor)
            };

            Assert.IsTrue(_instance.Equals(new HighlightCssProperty
            {
                Element = elem
            }));
        }

        [TestMethod]
        public void HasSameOuput_FalseTest()
        {
            var elem = new Highlight
            {
                Val = new EnumValue<HighlightColorValues>(HighlightColorValues.Yellow)
            };

            Assert.IsFalse(_instance.Equals(new HighlightCssProperty
            {
                Element = elem
            }));
        }
    }
}
