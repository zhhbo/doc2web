using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Plugins.Style.Properties
{
    [RunCssProperty(typeof(RunFonts))]
    public class RunFontsCssProperty : CssProperty<RunFonts>
    {
        private IThemeFontsProvider _themeFontProvider;
        private string[] _fontFamilies;

        public RunFontsCssProperty(IThemeFontsProvider themeFontProvider)
        {
            _themeFontProvider = themeFontProvider;
        }

        public string[] FontFaces
        {
            get
            {
                if (_fontFamilies == null)
                    _fontFamilies = BuildFontFamilyList();
                return _fontFamilies;
            }
        }

        public string FontFamilies => String.Join(", ", FontFaces.Where(x => x != null).Distinct());

        public override void InsertCss(CssData cssData)
        {
            string cleanFontFamilies = FontFamilies;
            if (cleanFontFamilies.Length == 0) return;
            cssData.AddAttribute(Selector, "font-family", String.Join(", ", cleanFontFamilies));
        }


        public override void Extends(CssProperty<RunFonts> parent)
        {
            if (parent is RunFontsCssProperty other)
            {
                var parentsFontFaces = other.FontFaces;
                var current = FontFaces;
                for(int i = 0; i < 4; i++)
                {
                    if (current[i] == null && parentsFontFaces[i] != null)
                        current[i] = parentsFontFaces[i];
                }
            }
        }

        private string[] BuildFontFamilyList()
        {
            var results = new string[4];
            var fontValues = FontValues;
            var themeFontValues = ThemeFontValues;

            for(int i = 0; i< 4; i++)
            {
                var fontName = GetRightFontFamily(fontValues[i], themeFontValues[i]);
                if (fontName != null && fontName != "")
                    results[i] = fontName;
            }

            return results;
        }

        private string[] FontValues =>
            new string[4]
            {
                Element.Ascii?.Value,
                Element.ComplexScript?.Value,
                Element.EastAsia?.Value,
                Element.HighAnsi?.Value,
            };

        private ThemeFontValues?[] ThemeFontValues =>
            new ThemeFontValues?[4] 
            {
                Element.AsciiTheme?.Value,
                Element.ComplexScriptTheme?.Value,
                Element.EastAsiaTheme?.Value,
                Element.HighAnsiTheme?.Value,
            };

        private string GetRightFontFamily(string fontValue, ThemeFontValues? themeFontValue)
        {
            if (fontValue != null) return fontValue;
            var themeFont =  TryGetFontFromTheme(themeFontValue);
            if (themeFont != null) return themeFont;
            return "";
        }

        private string TryGetFontFromTheme(ThemeFontValues? value)
        {
            try
            {
                if (!value.HasValue) return null;
                return _themeFontProvider.GetFontFace(value.Value);
            }
            catch
            {
                return null;
            }
        }

        public override short GetSpecificHashcode()
        {
            var asciiFont = GetRightFontFamily(
                Element.Ascii?.Value,
                Element.AsciiTheme?.Value
            );

            if (asciiFont == "") return 0;

            return (short)GetRightFontFamily(
                Element.Ascii?.Value,
                Element.AsciiTheme?.Value
            ).GetHashCode();
        }

        public override bool HaveSameOuput(RunFonts element)
        {
            var other = new RunFontsCssProperty(_themeFontProvider) { Element = element };
            return other.FontFamilies == FontFamilies;
        }
    }
}
