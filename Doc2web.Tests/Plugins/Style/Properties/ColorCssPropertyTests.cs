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
        private IThemeColorProvider _themeColorProvider;
        private ColorCssProperty _instance;

        [TestInitialize]
        public void Initialize()
        {
            _themeColorProvider = Substitute.For<IThemeColorProvider>();
            _instance = new ColorCssProperty(_themeColorProvider);
            _instance.Element = new Color();
            _instance.Selector = "p";
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
            var elem = new Color { Val = new StringValue("000000") };
            _instance.Element = elem.CloneNode(true) as Color;

            Assert.IsTrue(_instance.HaveSameOuput(elem));
        }
    }
}
