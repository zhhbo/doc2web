using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Doc2web.Plugins.Style;

namespace Doc2web.Tests.Plugins
{
    [TestClass]
    public class ThemeColorProviderTests
    {
        private ThemeColorsProvider _instance;

        private ColorScheme GenerateColorScheme()
        {
            ColorScheme colorScheme1 = new ColorScheme() { Name = "Office" };

            Dark1Color dark1Color1 = new Dark1Color();
            SystemColor systemColor1 = new SystemColor() { Val = SystemColorValues.WindowText, LastColor = "000000" };

            dark1Color1.Append(systemColor1);

            Dark2Color dark2Color1 = new Dark2Color();
            RgbColorModelHex rgbColorModelHex1 = new RgbColorModelHex() { Val = "44546A" };

            dark2Color1.Append(rgbColorModelHex1);

            Light1Color light1Color1 = new Light1Color();
            SystemColor systemColor2 = new SystemColor() { Val = SystemColorValues.Window, LastColor = "FFFFFF" };

            light1Color1.Append(systemColor2);

            Light2Color light2Color1 = new Light2Color();
            RgbColorModelHex rgbColorModelHex2 = new RgbColorModelHex() { Val = "E7E6E6" };

            light2Color1.Append(rgbColorModelHex2);

            Accent1Color accent1Color1 = new Accent1Color();
            RgbColorModelHex rgbColorModelHex3 = new RgbColorModelHex() { Val = "4472C4" };

            accent1Color1.Append(rgbColorModelHex3);

            Accent2Color accent2Color1 = new Accent2Color();
            RgbColorModelHex rgbColorModelHex4 = new RgbColorModelHex() { Val = "ED7D31" };

            accent2Color1.Append(rgbColorModelHex4);

            Accent3Color accent3Color1 = new Accent3Color();
            RgbColorModelHex rgbColorModelHex5 = new RgbColorModelHex() { Val = "A5A5A5" };

            accent3Color1.Append(rgbColorModelHex5);

            Accent4Color accent4Color1 = new Accent4Color();
            RgbColorModelHex rgbColorModelHex6 = new RgbColorModelHex() { Val = "FFC000" };

            accent4Color1.Append(rgbColorModelHex6);

            Accent5Color accent5Color1 = new Accent5Color();
            RgbColorModelHex rgbColorModelHex7 = new RgbColorModelHex() { Val = "5B9BD5" };

            accent5Color1.Append(rgbColorModelHex7);

            Accent6Color accent6Color1 = new Accent6Color();
            RgbColorModelHex rgbColorModelHex8 = new RgbColorModelHex() { Val = "70AD47" };

            accent6Color1.Append(rgbColorModelHex8);

            var hyperlink1 = new DocumentFormat.OpenXml.Drawing.Hyperlink();
            RgbColorModelHex rgbColorModelHex9 = new RgbColorModelHex() { Val = "0563C1" };

            hyperlink1.Append(rgbColorModelHex9);

            FollowedHyperlinkColor followedHyperlinkColor1 = new FollowedHyperlinkColor();
            RgbColorModelHex rgbColorModelHex10 = new RgbColorModelHex() { Val = "954F72" };

            followedHyperlinkColor1.Append(rgbColorModelHex10);

            colorScheme1.Append(dark1Color1);
            colorScheme1.Append(light1Color1);
            colorScheme1.Append(dark2Color1);
            colorScheme1.Append(light2Color1);
            colorScheme1.Append(accent1Color1);
            colorScheme1.Append(accent2Color1);
            colorScheme1.Append(accent3Color1);
            colorScheme1.Append(accent4Color1);
            colorScheme1.Append(accent5Color1);
            colorScheme1.Append(accent6Color1);
            colorScheme1.Append(hyperlink1);
            colorScheme1.Append(followedHyperlinkColor1);
            return colorScheme1;
        }

        private Theme GenerateTheme()
        {
            Theme theme = new Theme() { Name = "Office Theme" };
            theme.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            ThemeElements themeElements = new ThemeElements();
            themeElements.Append(GenerateColorScheme());

            theme.Append(themeElements);
            return theme;
        }

        [TestInitialize]
        public void Initialize()
        {
            _instance = new ThemeColorsProvider(GenerateTheme());
        }

        [TestMethod]
        public void Getcolor_Dark1Test()
        {
            Test("000000", ThemeColorValues.Dark1);
        }

        [TestMethod]
        public void Getcolor_Dark2Test()
        {
            Test("44546A", ThemeColorValues.Dark2);
        }

        [TestMethod]
        public void Getcolor_Light1Test()
        {
            Test("FFFFFF", ThemeColorValues.Light1);
        }

        [TestMethod]
        public void Getcolor_Light2Test()
        {
            Test("E7E6E6", ThemeColorValues.Light2);
        }

        [TestMethod]
        public void Getcolor_Accent1Test()
        {
            Test("4472C4", ThemeColorValues.Accent1);
        }

        [TestMethod]
        public void Getcolor_Accent2Test()
        {
            Test("ED7D31", ThemeColorValues.Accent2);
        }

        [TestMethod]
        public void Getcolor_Accent3Test()
        {
            Test("A5A5A5", ThemeColorValues.Accent3);
        }

        [TestMethod]
        public void Getcolor_Accent4Test()
        {
            Test("FFC000", ThemeColorValues.Accent4);
        }

        [TestMethod]
        public void Getcolor_Accent5Test()
        {
            Test("5B9BD5", ThemeColorValues.Accent5);
        }

        [TestMethod]
        public void Getcolor_Accent6Test()
        {
            Test("70AD47", ThemeColorValues.Accent6);
        }

        [TestMethod]
        public void Getcolor_HyperlinkTest()
        {
            Test("0563C1", ThemeColorValues.Hyperlink);
        }

        [TestMethod]
        public void Getcolor_FolowedHyperlinkTest()
        {
            Test("954F72", ThemeColorValues.FollowedHyperlink);
        }

        private void Test(string expected, ThemeColorValues enumVal)
        {
            Assert.AreEqual(expected, _instance.GetColor(enumVal));
        }
    }
}
