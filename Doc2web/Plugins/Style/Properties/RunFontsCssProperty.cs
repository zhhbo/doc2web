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
        private string[] _fontFaces;

        public RunFontsCssProperty(IThemeFontsProvider themeFontProvider)
        {
            _themeFontProvider = themeFontProvider;
        }

        private RunFontsCssProperty() { }


        public string[] FontFaces
        {
            get
            {
                if (_fontFaces == null)
                    _fontFaces = BuildFontFacesList();
                return _fontFaces;
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

        private string[] BuildFontFacesList()
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
            for (int i = 0; i < 4; i++)
                if (FontFaces[i] != null) return (short)FontFaces[i].GetHashCode();
            return -1;
        }

        public override bool HaveSameOutput(ICssProperty element)
        {
            var other = element as RunFontsCssProperty;
            if (other == null) return false;

            var otherSet = other.FontFaces.Where(x => x != null).Distinct().ToArray();
            var mySet = FontFaces.Where(x => x != null).Distinct().ToArray();

            if (otherSet.Length != mySet.Length) return false;

            for (int i = 0; i < mySet.Length; i++)
                if (mySet[i] != otherSet[i]) return false;

            return true;
        }

        public override ICssProperty Clone()
        {
            var clone = new RunFontsCssProperty();
            clone.Selector = Selector;
            clone._fontFaces = new string[4];
            clone.Element = Element;
            FontFaces.CopyTo(clone._fontFaces, 0);
            clone._themeFontProvider = _themeFontProvider;
            return clone;
        }

    }
}
