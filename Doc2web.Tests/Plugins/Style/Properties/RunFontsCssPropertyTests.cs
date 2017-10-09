using System;
using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Properties;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

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
                Selector = "span.test"
            };
            _themeFontProvider
                .GetFontFace(Arg.Any<ThemeFontValues>())
                .Returns(x =>
                   Enum.GetName(
                       typeof(ThemeFontValues), 
                       x.Arg<ThemeFontValues>()));
        }


        [TestMethod]
        public void AsCss_FontTest()
        {
            SetFontsValues();

            var result = _instance.AsCss();

            Assert.AreEqual(
                "Arial, HighAnsi, EastAsia, ComplexScript", 
                result[""]["span.test"]["font-family"]
            );
        }

        [TestMethod]
        public void AsCss_ThemeFontTest()
        {
            SetThemeFontValues();

            var result = _instance.AsCss();

            Assert.AreEqual(
                "MajorAscii, MajorHighAnsi, MajorEastAsia, MajorBidi", 
                result[""]["span.test"]["font-family"]);
        }

        [TestMethod]
        public void AsCss_DistincFontTests()
        {
            SetFontsValues("Arial", "Arial", "Arial", "Arial");

            var result = _instance.AsCss();

            Assert.AreEqual("Arial", result[""]["span.test"]["font-family"]);
        }

        [TestMethod]
        public void AsCss_InlineOverThemeTest()
        {
            SetThemeFontValues();
            _instance.Element.Ascii = "Arial";
            _instance.Element.ComplexScript = "Winding";
            _instance.Element = _instance.Element;

            var result = _instance.AsCss();

            Assert.AreEqual(
                "Arial, MajorHighAnsi, MajorEastAsia, Winding",
                result[""]["span.test"]["font-family"]
            );

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
        public void Extends_InlineTest()
        {
            SetFontsValues("A1", null, "A2", null);
            var instanceA = _instance.Clone() as RunFontsCssProperty;
            SetFontsValues("A3", "B1", "A4", "B2");

            instanceA.Extends(_instance);

            Assert.AreEqual("A1, B1, A2, B2", instanceA.CreateFontFamilyValue());
        }

        [TestMethod]
        public void Extends_ConflictingTest()
        {
            SetFontsValues();
            var instanceA = _instance.Clone();
            SetThemeFontValues();

            _instance.Extends(instanceA);

            Assert.AreEqual(
                "MajorAscii, MajorHighAnsi, MajorEastAsia, MajorBidi", 
                _instance.CreateFontFamilyValue()
            );
        }

        [TestMethod]
        public void Extends_ThemeTest()
        {
            SetThemeFontValues(
                ThemeFontValues.MajorAscii, 
                null, 
                ThemeFontValues.MajorEastAsia, 
                null);
            var instanceA = _instance.Clone() as RunFontsCssProperty;
            Initialize();
            SetThemeFontValues(
                ThemeFontValues.MinorAscii, 
                ThemeFontValues.MinorHighAnsi, 
                ThemeFontValues.MinorEastAsia,
                ThemeFontValues.MinorBidi);

            instanceA.Extends(_instance);

            Assert.AreEqual(
                "MajorAscii, MinorHighAnsi, MajorEastAsia, MinorBidi",
                instanceA.CreateFontFamilyValue());
        }

        [TestMethod]
        public void SpecificHashCode()
        {
            string fontName = "Consolas";
            SetFontsValues(fontName);
            var instance1 = _instance.Clone() as RunFontsCssProperty;
            _themeFontProvider.GetFontFace(ThemeFontValues.MajorAscii).Returns(fontName);
            SetThemeFontValues(ThemeFontValues.MajorAscii);

            Assert.AreEqual(
                instance1.GetSpecificHashcode(), 
                _instance.GetSpecificHashcode());
        }

        [TestMethod]
        public void HaveSameOutput_TrueTest()
        {
            SetThemeFontValues();
            var instance1 = _instance.Clone();
            SetFontsValues("MajorAscii", "MajorHighAnsi", "MajorEastAsia", "MajorBidi");

            Assert.IsTrue(instance1.HaveSameOutput(_instance));
        }

        [TestMethod]
        public void HaveSameOutput_FalseTest()
        {
            SetFontsValues();
            var other = _instance.Clone();
            SetFontsValues("Arial", "MajorHighAnsi", "EastAsia", "ComplexScript");

            Assert.IsFalse(_instance.HaveSameOutput(other));
        }

        private void SetFontsValues(
            string ascii = "Arial",
            string highAnsi = "HighAnsi",
            string eastAsia = "EastAsia",
            string complex = "ComplexScript")
        {
            _instance.Element = new RunFonts
            {
                Ascii = ascii,
                HighAnsi = highAnsi,
                EastAsia = eastAsia,
                ComplexScript = complex
            };
        }


        private void SetThemeFontValues(
            ThemeFontValues? ascii = ThemeFontValues.MajorAscii,
            ThemeFontValues? ha = ThemeFontValues.MajorHighAnsi,
            ThemeFontValues? ea = ThemeFontValues.MajorEastAsia,
            ThemeFontValues? cs = ThemeFontValues.MajorBidi)
        {
            var element = new RunFonts();
            if (ascii.HasValue) element.AsciiTheme = ascii.Value;
            if (ha.HasValue) element.HighAnsiTheme = ha.Value;
            if (ea.HasValue) element.EastAsiaTheme = ea.Value;
            if (cs.HasValue) element.ComplexScriptTheme = cs.Value;
            _instance.Element = element;
        }
    }
}
