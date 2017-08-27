using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Drawing;
using System.Linq;

namespace Doc2web.Plugins.Style
{
    public class ThemeColorProvider : IThemeColorProvider
    {
        private Theme _theme;

        public ThemeColorProvider(Theme theme)
        {
            _theme = theme;
        }

        public string GetColor(ThemeColorValues value)
        {
            var elements = _theme.ThemeElements.ColorScheme;
            switch (value)
            {
                case ThemeColorValues.Accent1:
                    return GetColorAsString(elements.Accent1Color);
                case ThemeColorValues.Accent2:
                    return GetColorAsString(elements.Accent2Color);
                case ThemeColorValues.Accent3:
                    return GetColorAsString(elements.Accent3Color);
                case ThemeColorValues.Accent4:
                    return GetColorAsString(elements.Accent4Color);
                case ThemeColorValues.Accent5:
                    return GetColorAsString(elements.Accent5Color);
                case ThemeColorValues.Accent6:
                    return GetColorAsString(elements.Accent6Color);
                case ThemeColorValues.Dark1:
                    return GetColorAsString(elements.Dark1Color);
                case ThemeColorValues.Dark2:
                    return GetColorAsString(elements.Dark2Color);
                case ThemeColorValues.FollowedHyperlink:
                    return GetColorAsString(elements.FollowedHyperlinkColor);
                case ThemeColorValues.Hyperlink:
                    return GetColorAsString(elements.Hyperlink);
                case ThemeColorValues.Light1:
                    return GetColorAsString(elements.Light1Color);
                case ThemeColorValues.Light2:
                    return GetColorAsString(elements.Light2Color);
                default: return null;
            }
        }

        private string GetColorAsString(Color2Type element)
        {
            if (element == null) return "";
            string color = element.RgbColorModelHex?.Val?.Value;
            if (color != null) return color;
            color = element.SystemColor?.LastColor?.Value;
            if (color != null) return color;
            return "";
        }
        

    }
}
