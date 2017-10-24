using Doc2web.Plugins;
using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Properties;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Style.Properties
{
    [TestClass]
    public class UnderlineCssPropertyTests
    {
        private IThemeColorsProvider _themeColorProvider;
        private UnderlineCssProperty _instance;

        [TestInitialize]
        public void Initialize()
        {
            _themeColorProvider = Substitute.For<IThemeColorsProvider>();
            _instance = new UnderlineCssProperty(_themeColorProvider)
            {
                Element = new Underline(),
                Selector = "span.test"
            };
        }

        [TestMethod]
        public void AsCss_SimpleTest()
        {
            var expected = new CssData();
            expected.AddAttribute("span.test", "text-decoration", "underline solid");

            var data = _instance.AsCss();

            Assert.AreEqual(expected, data);
        }

        [TestMethod]
        public void AsCss_ColorTest()
        {
            var expected = new CssData();
            expected.AddAttribute("span.test", "text-decoration", "underline wavy #FFFF00");
            _instance.Element.Val = new EnumValue<UnderlineValues>(UnderlineValues.Wave);
            _instance.Element.Color = new StringValue("FFFF00");

            var data = _instance.AsCss();

            Assert.AreEqual(expected, data);
        }

        [TestMethod]
        public void AsCss_ValNoneTest()
        {
            var expected = new CssData();
            expected.AddAttribute("span.test", "text-decoration", "underline solid white");
            _instance.Element.Val = UnderlineValues.None;

            var data = _instance.AsCss();

            Assert.AreEqual(expected, data);
        }

        [TestMethod]
        public void AsCss_ThemeColorTest()
        {
            var expected = new CssData();
            expected.AddAttribute("span.test", "text-decoration", "underline dashed #000000");
            _themeColorProvider.GetColor(Arg.Is(ThemeColorValues.Dark1)).Returns("000000");
            _instance.Element.Val = new EnumValue<UnderlineValues>(UnderlineValues.DashedHeavy);
            _instance.Element.Color = new StringValue("FFFFFF");
            _instance.Element.ThemeColor = new EnumValue<ThemeColorValues>(ThemeColorValues.Dark1);

            var data = _instance.AsCss();

            Assert.AreEqual(expected, data);
        }

        [TestMethod]
        public void GetHashCode_EqualTest()
        {
            _instance.Element.Val = new EnumValue<UnderlineValues>(UnderlineValues.DotDash);
            _instance.Element.Color = new StringValue("FFFFFF");
            var other = new UnderlineCssProperty(_themeColorProvider)
            {
                Element = _instance.Element.CloneNode(true) as Underline
            };

            Assert.AreEqual(_instance.GetHashCode(), other.GetHashCode());
        }

        [TestMethod]
        public void GetHashCode_NotEqualTest()
        {
            _instance.Element.Val = new EnumValue<UnderlineValues>(UnderlineValues.DotDash);
            _instance.Element.Color = new StringValue("FFFFFF");
            var other = new UnderlineCssProperty(_themeColorProvider)
            {
                Element = _instance.Element.CloneNode(true) as Underline
            };
            other.Element.Color = null;

            Assert.AreNotEqual(_instance.GetHashCode(), other.GetHashCode());
        }

        [TestMethod]
        public void Equals_TrueTest()
        {
            _themeColorProvider.GetColor(Arg.Is(ThemeColorValues.Dark1)).Returns("000000");
            _instance.Element.Val = new EnumValue<UnderlineValues>(UnderlineValues.DotDash);
            _instance.Element.Color = new StringValue("FFFFFF");
            _instance.Element.ThemeColor = new EnumValue<ThemeColorValues>(ThemeColorValues.Dark1);
            var other = _instance.Element.CloneNode(true) as Underline;

            Assert.IsTrue(_instance.Equals(new UnderlineCssProperty(_themeColorProvider)
            {
                Element = other
            }));

        }

        [TestMethod]
        public void Equals_FalseTest()
        {
            _instance.Element.Val = new EnumValue<UnderlineValues>(UnderlineValues.DotDash);
            var other = _instance.Element.CloneNode(true) as Underline;
            other.Color = new StringValue("#FFFFFF");

            Assert.IsFalse(_instance.Equals(new UnderlineCssProperty(_themeColorProvider)
            {
                Element = other
            }));
        }

        [TestMethod]
        public void Clone_Test()
        {
            _instance.Element.Val = new EnumValue<UnderlineValues>(UnderlineValues.DotDash);
            var clone = _instance.Clone();

            Assert.AreNotSame(clone, _instance);
            Assert.AreEqual(clone, _instance);
            Assert.AreEqual(clone.AsCss(), _instance.AsCss());
        }

    }
}
