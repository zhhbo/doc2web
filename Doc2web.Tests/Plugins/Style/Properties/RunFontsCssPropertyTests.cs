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
    public class RunFontsCssPropertyTests
    {
        private IThemeFontsProvider _themeFontProvider;
        private RunFontsCssProperty _instance;

        [TestInitialize]
        public void Initialize()
        {
            _themeFontProvider = Substitute.For<IThemeFontsProvider>();
            _instance = new RunFontsCssProperty(_themeFontProvider)
            {
                Element = new RunFonts(),
                Selector = "span.test"
            };
        }


        [TestMethod]
        public void AsCss_FontTest()
        {
            SetFontsValues();

            var result = _instance.AsCss();

            Assert.AreEqual(ExpectedValues, result[""]["span.test"]["font-family"]);
        }

        [TestMethod]
        public void AsCss_ThemeFontTest()
        {
            SetThemeFontValues();

            var result = _instance.AsCss();

            Assert.AreEqual(ExpectedThemeValues, result[""]["span.test"]["font-family"]);
        }

        [TestMethod]
        public void AsCss_DistincFontTests()
        {
            SetFontsValues("Arial", "Arial", "Arial", "Arial");

            var result = _instance.AsCss();

            Assert.AreEqual("Arial", result[""]["span.test"]["font-family"]);
        }

        [TestMethod]
        public void AsCss_NoFontTests()
        {
            SetFontsValues(null, null, null, null);

            var result = _instance.AsCss();
            result.AddAttribute("span.test", "some", "val");

            Assert.IsFalse(result[""]["span.test"].ContainsKey("font-family"));
        }

        [TestMethod]
        public void Extends_Test()
        {
            SetFontsValues("A1", null, "A2", null);
            var instanceA = _instance;
            Initialize();
            SetFontsValues("A3", "B1", "A4", "B2");
            var instanceB = _instance;

            instanceA.Extends(instanceB);

            Assert.AreEqual("A1, B1, A2, B2", instanceA.FontFamilies);
        }

        [TestMethod]
        public void SpecificHashcode_Test()
        {
            _instance.Element.Ascii = new StringValue("A");
            var instance2 = new RunFontsCssProperty(_themeFontProvider)
            {
                Element = new RunFonts(),
            };
            instance2.Element.AsciiTheme = new EnumValue<ThemeFontValues>(ThemeFontValues.MajorAscii);
            _themeFontProvider.GetFontFace(instance2.Element.AsciiTheme.Value).Returns("A");

            Assert.AreEqual(instance2.GetSpecificHashcode(), _instance.GetSpecificHashcode());
        }

        [TestMethod]
        public void SpecificHashcode_NullTest()
        {
            _instance.Element.Ascii = null;
            _instance.Element.AsciiTheme = null;

            var result = _instance.GetSpecificHashcode();
            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void HaveSameOuput_TrueTest()
        {
            SetFontsValues();
            var otherElement = _instance.Element.CloneNode(true) as RunFonts;
            otherElement.ComplexScript = null;
            otherElement.ComplexScriptTheme = new EnumValue<ThemeFontValues>(ThemeFontValues.MajorAscii);
            _themeFontProvider
                .GetFontFace(ThemeFontValues.MajorAscii)
                .Returns(_instance.Element.ComplexScript.Value);

            Assert.IsTrue(_instance.HaveSameOuput(otherElement));
        }

        [TestMethod]
        public void HaveSameOuput_False1Test()
        {
            SetFontsValues();
            var otherElement = _instance.Element.CloneNode(true) as RunFonts;
            otherElement.ComplexScript = new StringValue("Different");

            Assert.IsFalse(_instance.HaveSameOuput(otherElement));
        }

        [TestMethod]
        public void HaveSameOuput_False2Test()
        {
            SetFontsValues();
            var otherElement = _instance.Element.CloneNode(true) as RunFonts;
            otherElement.HighAnsi = null;

            Assert.IsFalse(_instance.HaveSameOuput(otherElement));
        }

        [TestMethod]
        public void Clone_Test()
        {
            Initialize();
            SetFontsValues("A3", "B1", "A4", "B2");

            var clone = _instance.Clone();

            Assert.AreEqual(clone, _instance);
            Assert.AreSame(clone.OpenXmlElement, _instance.OpenXmlElement);
            Assert.AreNotSame(clone, _instance);
            Assert.AreEqual(clone.AsCss(), _instance.AsCss());

        }

        static string ExpectedValues = 
            "Arial, ComplexScript, EastAsia, HighAnsi";
        static string ExpectedThemeValues = 
            "Arial Theme, ComplexScript Theme, EastAsia Theme, HighAnsi Theme";

        private void SetFontsValues(
            string ascii="Arial",
            string complex="ComplexScript",
            string eastAsia="EastAsia",
            string highAnsi="HighAnsi")
        {
            _instance.Element.Ascii = new StringValue(ascii);
            _instance.Element.ComplexScript = new StringValue(complex);
            _instance.Element.EastAsia = new StringValue(eastAsia);
            _instance.Element.HighAnsi = new StringValue(highAnsi);
        }


        private void SetThemeFontValues()
        {
            var ascii = new EnumValue<ThemeFontValues>(ThemeFontValues.MajorAscii);
            var cs = new EnumValue<ThemeFontValues>(ThemeFontValues.MajorBidi);
            var ea = new EnumValue<ThemeFontValues>(ThemeFontValues.MinorEastAsia);
            var ha = new EnumValue<ThemeFontValues>(ThemeFontValues.MinorHighAnsi);

            _themeFontProvider.GetFontFace(ascii).Returns("Arial Theme");
            _themeFontProvider.GetFontFace(cs).Returns("ComplexScript Theme");
            _themeFontProvider.GetFontFace(ea).Returns("EastAsia Theme");
            _themeFontProvider.GetFontFace(ha).Returns("HighAnsi Theme");

            _instance.Element.AsciiTheme = ascii;
            _instance.Element.ComplexScriptTheme = cs;
            _instance.Element.EastAsiaTheme = ea;
            _instance.Element.HighAnsiTheme = ha;
        }
    }
}
