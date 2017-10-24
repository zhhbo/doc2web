using Doc2web.Plugins;
using Doc2web.Plugins.Style.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using Doc2web.Plugins.Style;
using DocumentFormat.OpenXml;

namespace Doc2web.Tests.Plugins.Style.Properties
{
    [TestClass]
    public class ColorCssPropertyTests
    {
        private IThemeColorsProvider _themeColorProvider;
        private ColorCssProperty _instance;

        [TestInitialize]
        public void Initialize()
        {
            _themeColorProvider = Substitute.For<IThemeColorsProvider>();
            _instance = new ColorCssProperty(_themeColorProvider)
            {
                Element = new Color(),
                Selector = "p"
            };
        }

        [TestMethod]
        public void AsCss_ThemeColorAsciiTest()
        {
            var expectedCss = new CssData();
            expectedCss.AddAttribute("p", "color", "#FF00FF");
            _themeColorProvider.GetColor(ThemeColorValues.Accent1).Returns("FF00FF");
            _instance.Element.ThemeColor = new DocumentFormat.OpenXml.EnumValue<ThemeColorValues>(ThemeColorValues.Accent1);

            var result = _instance.AsCss();

            Assert.AreEqual(expectedCss, result);
        }

        [TestMethod]
        public void AsCss_ThemeColorNameTest()
        {
            var expectedCss = new CssData();
            expectedCss.AddAttribute("p", "color", "red");
            _themeColorProvider.GetColor(ThemeColorValues.Accent1).Returns("red");
            _instance.Element.ThemeColor = new DocumentFormat.OpenXml.EnumValue<ThemeColorValues>(ThemeColorValues.Accent1);

            var result = _instance.AsCss();

            Assert.AreEqual(expectedCss, result);
        }

        [TestMethod]
        public void AsCss_ValAsciiTest()
        {
            var expectedCss = new CssData();
            expectedCss.AddAttribute("p", "color", "#000000");
            _instance.Element.Val = new StringValue("000000");

            var result = _instance.AsCss();

            Assert.AreEqual(expectedCss, result);
        }


        [TestMethod]
        public void AsCss_ValColorNameTest()
        {
            var expectedCss = new CssData();
            expectedCss.AddAttribute("p", "color", "red");
            _instance.Element.Val = new StringValue("red");

            var result = _instance.AsCss();

            Assert.AreEqual(expectedCss, result);
        }

        [TestMethod]
        public void HasSameOutput_TrueTest()
        {
            Assert.IsTrue(_instance.Equals(new ColorCssProperty(_themeColorProvider)
            {
                Element = _instance.Element.CloneNode(true) as Color
            }));
        }

        [TestMethod]
        public void HashCode_Test()
        {
            var elem = new Color { Val = new StringValue("123456") };
            _instance.Element = elem.CloneNode(true) as Color;

            var result = _instance.GetHashCode();
            var expected = Convert.ToUInt32("123456", 16);

            Assert.AreEqual((int)expected, result);
        }

        [TestMethod]
        public void Clone_Test()
        {
            var elem = new Color { Val = new StringValue("123456") };
            _instance.Element = elem.CloneNode(true) as Color;

            var clone = _instance.Clone();

            Assert.AreNotSame(clone, _instance);
            Assert.AreEqual(clone, _instance);
            Assert.AreEqual(clone.AsCss(), _instance.AsCss());
        }
    }
}
