using DocumentFormat.OpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Plugins.Style
{
    public class ThemeFontsProvider : IThemeFontsProvider
    {
        private Theme _theme;

        public ThemeFontsProvider(Theme theme)
        {
            _theme = theme;
        }

        public string GetFontFace(ThemeFontValues value)
        {
            var face = GetFontType(value)?.Typeface?.Value;
            if (face == null)
                throw new ArgumentException("Not typefaces are associated with this font");
            return face;
        }

        public TextFontType GetFontType(ThemeFontValues value)
        {
            switch (value)
            {
                case ThemeFontValues.MajorAscii: return Major?.LatinFont;
                case ThemeFontValues.MajorHighAnsi: return Major?.LatinFont;
                case ThemeFontValues.MajorBidi: return Major?.ComplexScriptFont;
                case ThemeFontValues.MajorEastAsia: return Major?.EastAsianFont;
                //case ThemeFontValues.MajorHighAnsi: return null;
                case ThemeFontValues.MinorAscii: return Minor?.LatinFont;
                case ThemeFontValues.MinorHighAnsi: return Minor?.LatinFont;
                case ThemeFontValues.MinorBidi: return Minor?.ComplexScriptFont;
                case ThemeFontValues.MinorEastAsia: return Minor?.EastAsianFont;
                //case ThemeFontValues.MinorHighAnsi: return null;
                default: return null;
            }
        }

        private FontScheme Fonts => _theme.ThemeElements.FontScheme;
        private FontCollectionType Major => Fonts.MajorFont;
        private FontCollectionType Minor => Fonts.MinorFont;
    }
}
