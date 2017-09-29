using Microsoft.VisualStudio.TestTools.UnitTesting;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml;

using System;
using System.Collections.Generic;
using System.Text;
using Doc2web.Plugins.Style;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Tests.Plugins.Style
{
    [TestClass]
    public class ThemeFontsProviderTests
    {
        private ThemeFontsProvider _instance;

        [TestInitialize]
        public void Initialize()
        {
            _instance = new ThemeFontsProvider(GenerageTheme());
        }


        [TestMethod]
        public void GetFont_MajorAsciiTest()
        {
            var result = _instance.GetFontFace(ThemeFontValues.MajorAscii);
            Assert.AreEqual("Cambria", result);
        }

        [TestMethod]
        public void GetFont_MinorAsciiTest()
        {
            var result = _instance.GetFontFace(ThemeFontValues.MinorAscii);
            Assert.AreEqual("Calibri", result);
        }

        [TestMethod]
        public void GetFont_MajorBidiTest()
        {
            var result = _instance.GetFontFace(ThemeFontValues.MajorBidi);
            Assert.AreEqual("Courrier", result);
        }

        [TestMethod]
        public void GetFont_MinorBidiTest()
        {
            var result = _instance.GetFontFace(ThemeFontValues.MinorBidi);
            Assert.AreEqual("Courrier New", result);
        }

        [TestMethod]
        public void GetFont_MajorEastAsianTest()
        {
            var result = _instance.GetFontFace(ThemeFontValues.MajorEastAsia);
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void GetFont_MinorEastAsianTest()
        {
            var result = _instance.GetFontFace(ThemeFontValues.MinorEastAsia);
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void GetFont_MajorHighAnsiTest()
        {
            var result = _instance.GetFontFace(ThemeFontValues.MajorHighAnsi);
            Assert.AreEqual("Cambria", result);
        }

        [TestMethod]
        public void GetFont_MinorHighAnsiTest()
        {
            var result = _instance.GetFontFace(ThemeFontValues.MinorHighAnsi);
            Assert.AreEqual("Calibri", result);
        }


        private Theme GenerageTheme() =>
            new Theme
            {
                ThemeElements = new ThemeElements
                {
                    FontScheme = GenerateFontScheme()
                }
            };

        // Creates an FontScheme instance and adds its children.
        private FontScheme GenerateFontScheme()
        {
            FontScheme fontScheme1 = new FontScheme() { Name = "Office" };

            MajorFont majorFont1 = new MajorFont();
            LatinFont latinFont1 = new LatinFont() { Typeface = "Cambria" };
            EastAsianFont eastAsianFont1 = new EastAsianFont() { Typeface = "" };
            ComplexScriptFont complexScriptFont1 = new ComplexScriptFont() { Typeface = "Courrier" };
            SupplementalFont supplementalFont1 = new SupplementalFont() { Script = "Jpan", Typeface = "ＭＳ ゴシック" };
            SupplementalFont supplementalFont2 = new SupplementalFont() { Script = "Hang", Typeface = "맑은 고딕" };
            SupplementalFont supplementalFont3 = new SupplementalFont() { Script = "Hans", Typeface = "宋体" };
            SupplementalFont supplementalFont4 = new SupplementalFont() { Script = "Hant", Typeface = "新細明體" };
            SupplementalFont supplementalFont5 = new SupplementalFont() { Script = "Arab", Typeface = "Times New Roman" };
            SupplementalFont supplementalFont6 = new SupplementalFont() { Script = "Hebr", Typeface = "Times New Roman" };
            SupplementalFont supplementalFont7 = new SupplementalFont() { Script = "Thai", Typeface = "Angsana New" };
            SupplementalFont supplementalFont8 = new SupplementalFont() { Script = "Ethi", Typeface = "Nyala" };
            SupplementalFont supplementalFont9 = new SupplementalFont() { Script = "Beng", Typeface = "Vrinda" };
            SupplementalFont supplementalFont10 = new SupplementalFont() { Script = "Gujr", Typeface = "Shruti" };
            SupplementalFont supplementalFont11 = new SupplementalFont() { Script = "Khmr", Typeface = "MoolBoran" };
            SupplementalFont supplementalFont12 = new SupplementalFont() { Script = "Knda", Typeface = "Tunga" };
            SupplementalFont supplementalFont13 = new SupplementalFont() { Script = "Guru", Typeface = "Raavi" };
            SupplementalFont supplementalFont14 = new SupplementalFont() { Script = "Cans", Typeface = "Euphemia" };
            SupplementalFont supplementalFont15 = new SupplementalFont() { Script = "Cher", Typeface = "Plantagenet Cherokee" };
            SupplementalFont supplementalFont16 = new SupplementalFont() { Script = "Yiii", Typeface = "Microsoft Yi Baiti" };
            SupplementalFont supplementalFont17 = new SupplementalFont() { Script = "Tibt", Typeface = "Microsoft Himalaya" };
            SupplementalFont supplementalFont18 = new SupplementalFont() { Script = "Thaa", Typeface = "MV Boli" };
            SupplementalFont supplementalFont19 = new SupplementalFont() { Script = "Deva", Typeface = "Mangal" };
            SupplementalFont supplementalFont20 = new SupplementalFont() { Script = "Telu", Typeface = "Gautami" };
            SupplementalFont supplementalFont21 = new SupplementalFont() { Script = "Taml", Typeface = "Latha" };
            SupplementalFont supplementalFont22 = new SupplementalFont() { Script = "Syrc", Typeface = "Estrangelo Edessa" };
            SupplementalFont supplementalFont23 = new SupplementalFont() { Script = "Orya", Typeface = "Kalinga" };
            SupplementalFont supplementalFont24 = new SupplementalFont() { Script = "Mlym", Typeface = "Kartika" };
            SupplementalFont supplementalFont25 = new SupplementalFont() { Script = "Laoo", Typeface = "DokChampa" };
            SupplementalFont supplementalFont26 = new SupplementalFont() { Script = "Sinh", Typeface = "Iskoola Pota" };
            SupplementalFont supplementalFont27 = new SupplementalFont() { Script = "Mong", Typeface = "Mongolian Baiti" };
            SupplementalFont supplementalFont28 = new SupplementalFont() { Script = "Viet", Typeface = "Times New Roman" };
            SupplementalFont supplementalFont29 = new SupplementalFont() { Script = "Uigh", Typeface = "Microsoft Uighur" };
            SupplementalFont supplementalFont30 = new SupplementalFont() { Script = "Geor", Typeface = "Sylfaen" };

            majorFont1.Append(latinFont1);
            majorFont1.Append(eastAsianFont1);
            majorFont1.Append(complexScriptFont1);
            majorFont1.Append(supplementalFont1);
            majorFont1.Append(supplementalFont2);
            majorFont1.Append(supplementalFont3);
            majorFont1.Append(supplementalFont4);
            majorFont1.Append(supplementalFont5);
            majorFont1.Append(supplementalFont6);
            majorFont1.Append(supplementalFont7);
            majorFont1.Append(supplementalFont8);
            majorFont1.Append(supplementalFont9);
            majorFont1.Append(supplementalFont10);
            majorFont1.Append(supplementalFont11);
            majorFont1.Append(supplementalFont12);
            majorFont1.Append(supplementalFont13);
            majorFont1.Append(supplementalFont14);
            majorFont1.Append(supplementalFont15);
            majorFont1.Append(supplementalFont16);
            majorFont1.Append(supplementalFont17);
            majorFont1.Append(supplementalFont18);
            majorFont1.Append(supplementalFont19);
            majorFont1.Append(supplementalFont20);
            majorFont1.Append(supplementalFont21);
            majorFont1.Append(supplementalFont22);
            majorFont1.Append(supplementalFont23);
            majorFont1.Append(supplementalFont24);
            majorFont1.Append(supplementalFont25);
            majorFont1.Append(supplementalFont26);
            majorFont1.Append(supplementalFont27);
            majorFont1.Append(supplementalFont28);
            majorFont1.Append(supplementalFont29);
            majorFont1.Append(supplementalFont30);

            MinorFont minorFont1 = new MinorFont();
            LatinFont latinFont2 = new LatinFont() { Typeface = "Calibri" };
            EastAsianFont eastAsianFont2 = new EastAsianFont() { Typeface = "" };
            ComplexScriptFont complexScriptFont2 = new ComplexScriptFont() { Typeface = "Courrier New" };
            SupplementalFont supplementalFont31 = new SupplementalFont() { Script = "Jpan", Typeface = "ＭＳ 明朝" };
            SupplementalFont supplementalFont32 = new SupplementalFont() { Script = "Hang", Typeface = "맑은 고딕" };
            SupplementalFont supplementalFont33 = new SupplementalFont() { Script = "Hans", Typeface = "宋体" };
            SupplementalFont supplementalFont34 = new SupplementalFont() { Script = "Hant", Typeface = "新細明體" };
            SupplementalFont supplementalFont35 = new SupplementalFont() { Script = "Arab", Typeface = "Arial" };
            SupplementalFont supplementalFont36 = new SupplementalFont() { Script = "Hebr", Typeface = "Arial" };
            SupplementalFont supplementalFont37 = new SupplementalFont() { Script = "Thai", Typeface = "Cordia New" };
            SupplementalFont supplementalFont38 = new SupplementalFont() { Script = "Ethi", Typeface = "Nyala" };
            SupplementalFont supplementalFont39 = new SupplementalFont() { Script = "Beng", Typeface = "Vrinda" };
            SupplementalFont supplementalFont40 = new SupplementalFont() { Script = "Gujr", Typeface = "Shruti" };
            SupplementalFont supplementalFont41 = new SupplementalFont() { Script = "Khmr", Typeface = "DaunPenh" };
            SupplementalFont supplementalFont42 = new SupplementalFont() { Script = "Knda", Typeface = "Tunga" };
            SupplementalFont supplementalFont43 = new SupplementalFont() { Script = "Guru", Typeface = "Raavi" };
            SupplementalFont supplementalFont44 = new SupplementalFont() { Script = "Cans", Typeface = "Euphemia" };
            SupplementalFont supplementalFont45 = new SupplementalFont() { Script = "Cher", Typeface = "Plantagenet Cherokee" };
            SupplementalFont supplementalFont46 = new SupplementalFont() { Script = "Yiii", Typeface = "Microsoft Yi Baiti" };
            SupplementalFont supplementalFont47 = new SupplementalFont() { Script = "Tibt", Typeface = "Microsoft Himalaya" };
            SupplementalFont supplementalFont48 = new SupplementalFont() { Script = "Thaa", Typeface = "MV Boli" };
            SupplementalFont supplementalFont49 = new SupplementalFont() { Script = "Deva", Typeface = "Mangal" };
            SupplementalFont supplementalFont50 = new SupplementalFont() { Script = "Telu", Typeface = "Gautami" };
            SupplementalFont supplementalFont51 = new SupplementalFont() { Script = "Taml", Typeface = "Latha" };
            SupplementalFont supplementalFont52 = new SupplementalFont() { Script = "Syrc", Typeface = "Estrangelo Edessa" };
            SupplementalFont supplementalFont53 = new SupplementalFont() { Script = "Orya", Typeface = "Kalinga" };
            SupplementalFont supplementalFont54 = new SupplementalFont() { Script = "Mlym", Typeface = "Kartika" };
            SupplementalFont supplementalFont55 = new SupplementalFont() { Script = "Laoo", Typeface = "DokChampa" };
            SupplementalFont supplementalFont56 = new SupplementalFont() { Script = "Sinh", Typeface = "Iskoola Pota" };
            SupplementalFont supplementalFont57 = new SupplementalFont() { Script = "Mong", Typeface = "Mongolian Baiti" };
            SupplementalFont supplementalFont58 = new SupplementalFont() { Script = "Viet", Typeface = "Arial" };
            SupplementalFont supplementalFont59 = new SupplementalFont() { Script = "Uigh", Typeface = "Microsoft Uighur" };
            SupplementalFont supplementalFont60 = new SupplementalFont() { Script = "Geor", Typeface = "Sylfaen" };

            minorFont1.Append(latinFont2);
            minorFont1.Append(eastAsianFont2);
            minorFont1.Append(complexScriptFont2);
            minorFont1.Append(supplementalFont31);
            minorFont1.Append(supplementalFont32);
            minorFont1.Append(supplementalFont33);
            minorFont1.Append(supplementalFont34);
            minorFont1.Append(supplementalFont35);
            minorFont1.Append(supplementalFont36);
            minorFont1.Append(supplementalFont37);
            minorFont1.Append(supplementalFont38);
            minorFont1.Append(supplementalFont39);
            minorFont1.Append(supplementalFont40);
            minorFont1.Append(supplementalFont41);
            minorFont1.Append(supplementalFont42);
            minorFont1.Append(supplementalFont43);
            minorFont1.Append(supplementalFont44);
            minorFont1.Append(supplementalFont45);
            minorFont1.Append(supplementalFont46);
            minorFont1.Append(supplementalFont47);
            minorFont1.Append(supplementalFont48);
            minorFont1.Append(supplementalFont49);
            minorFont1.Append(supplementalFont50);
            minorFont1.Append(supplementalFont51);
            minorFont1.Append(supplementalFont52);
            minorFont1.Append(supplementalFont53);
            minorFont1.Append(supplementalFont54);
            minorFont1.Append(supplementalFont55);
            minorFont1.Append(supplementalFont56);
            minorFont1.Append(supplementalFont57);
            minorFont1.Append(supplementalFont58);
            minorFont1.Append(supplementalFont59);
            minorFont1.Append(supplementalFont60);

            fontScheme1.Append(majorFont1);
            fontScheme1.Append(minorFont1);
            return fontScheme1;
        }


    }
}
