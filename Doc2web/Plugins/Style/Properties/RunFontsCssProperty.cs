using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Properties
{
    public class RunFontsCssProperty : CssProperty<RunFonts>
    {
        private IThemeFontsProvider _themeFontProvider;

        public RunFontsCssProperty(IThemeFontsProvider themeFontProvider)
        {
            _themeFontProvider = themeFontProvider;
        }

        public override void InsertCss(CssData cssData)
        {
            var fontFamilyList = BuildFontFamilyList();
            cssData.AddAttribute(Selector, "font-family", String.Join(", ", fontFamilyList));
        }

        private List<string> BuildFontFamilyList()
        {
            var results = new List<string>(4);
            var fontValues = FontValues;
            var themeFontValues = ThemeFontValues;

            for(int i = 0; i< 4; i++)
            {
                var nextFont = GetRightFontFamily(fontValues[i], themeFontValues[i]);
                if (nextFont != null) results.Add(nextFont);
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
            return TryGetFontFromTheme(themeFontValue);
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
            return (short)GetRightFontFamily(
                Element.Ascii?.Value,
                Element.AsciiTheme?.Value
            ).GetHashCode();
        }

        public override bool HaveSameOuput(RunFonts element)
        {
            var other = new RunFontsCssProperty(_themeFontProvider) { Element = element };
            var otherList = other.BuildFontFamilyList();
            var myList = BuildFontFamilyList();
            return AreEqualStringLists(otherList, myList);
        }

        private static bool AreEqualStringLists(List<string> a, List<string> b)
        {
            if (a.Count != b.Count) return false;
            for (int i = 0; i < b.Count; i++)
                if (a[i] != b[i])
                    return false;
            return true;
        }
    }
}
