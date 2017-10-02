using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.RegularExpressions;

namespace Doc2web.Plugins.Style.Properties
{
    /* Supported attributeS:
     * themeColor, val
     * Not supported attributes:
     * themeTint, themeShade */

    [RunCssProperty(typeof(Color))]
    public class ColorCssProperty : CssProperty<Color>
    {
        private static Regex hexadecimalRegex = new Regex(@"^[0-9a-f]{6}|[0-9A-F]{6}$", RegexOptions.Compiled);

        private IThemeColorsProvider _themeColorProvider;

        public ColorCssProperty(IThemeColorsProvider themeColorProvider)
        {
            _themeColorProvider = themeColorProvider;
        }

        private string ColorTheme
        {
            get
            {
                if (Element?.ThemeColor?.Value != null)
                    return _themeColorProvider.GetColor(Element.ThemeColor.Value);

                return null;
            }
        }

        private string ColorVal => Element?.Val?.Value;

        private string ParseColor(string rawColor)
        {
            if (hexadecimalRegex.IsMatch(rawColor)) return "#" + rawColor;
            return rawColor;
        }

        private string Color
        {
            get
            {
                if (ColorTheme != null) return ParseColor(ColorTheme);
                if (ColorVal != null) return ParseColor(ColorVal);
                return null;
            }
        }

        public override void InsertCss(CssData cssData)
        {
            string color = Color;
            if (color != null)
            {
                cssData.AddAttribute(Selector, "color", color);
            }
        }

        public override bool HaveSameOuput(Color element)
        {
            var other = new ColorCssProperty(_themeColorProvider) { Element = element };
            return other.Color == Color;
        }


        public override short GetSpecificHashcode() => (short)Color.Substring(1).GetHashCode();

        public override ICssProperty Clone() =>
            new ColorCssProperty(_themeColorProvider)
            {
                Element = Element,
                Selector = Selector
            };

    }
}
