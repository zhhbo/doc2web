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

    public class ColorCssProperty : BaseCssProperty<Color>
    {
        private static Regex hexadecimalRegex = new Regex(@"^[0-9a-f]{6}|[0-9A-F]{6}$", RegexOptions.Compiled);

        private IThemeColorProvider _themeColorProvider;

        public ColorCssProperty(IThemeColorProvider themeColorProvider)
        {
            _themeColorProvider = themeColorProvider;
        }

        private string ThemeColor
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

        public override CssData AsCss()
        {
            var cssData = new CssData();
            var themeColor = ThemeColor;
            if (themeColor != null)
            {
                cssData.AddAttribute(Selector, "color", ParseColor(themeColor));
            }
            else if (ColorVal != null) 
            {
                cssData.AddAttribute(Selector, "color", ParseColor(ColorVal));
            }
            return cssData;
        }

        public override int CompareTo(Color element)
        {
            throw new NotImplementedException();
        }
    }
}
