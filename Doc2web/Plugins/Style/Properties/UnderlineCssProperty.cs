﻿using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Properties
{
    [RunCssProperty(typeof(Underline))]
    public class UnderlineCssProperty : CssProperty<Underline>
    {
        private IThemeColorsProvider _themeColorProvider;

        public UnderlineCssProperty(IThemeColorsProvider themeColorProvider) : base()
        {
            _themeColorProvider = themeColorProvider;
        }

        public override void InsertCss(CssData cssData)
        {
            cssData.AddAttribute(Selector, "text-decoration", TextDecoration);
        }

        private string TextDecoration
        {
            get
            {
                var style = Style;
                if (style == "none") return "underline solid white"; // bug fix for chrome, none is not recognized.
                else return $"underline {Style} {Color}".TrimEnd();
            }
        }

        public override int GetHashCode() => (Element.Val?.Value, Color).GetHashCode();

        public override bool Equals(ICssProperty element)
        {
            if (element is UnderlineCssProperty other)
            {
                return other.Style == Style && other.Color == Color;
            }
            return false;
        }

        private string Style
        {
            get
            {
                switch(Element.Val?.Value)
                {
                    case UnderlineValues.Dash:
                    case UnderlineValues.DashDotDotHeavy:
                    case UnderlineValues.DashDotHeavy:
                    case UnderlineValues.DashedHeavy:
                    case UnderlineValues.DashLongHeavy:
                        return "dashed";
                    case UnderlineValues.DotDash:
                    case UnderlineValues.DotDotDash:
                    case UnderlineValues.Dotted:
                    case UnderlineValues.DottedHeavy:
                        return "dotted";
                    case UnderlineValues.Double:
                        return "double";
                    case UnderlineValues.Wave:
                    case UnderlineValues.WavyDouble:
                    case UnderlineValues.WavyHeavy:
                        return "wavy";
                    case UnderlineValues.None:
                        return "none";
                    default:
                        return "solid";
                }
            }
        }

        private string Color
        {
            get
            {
                if (Element.ThemeColor?.Value != null)
                    return "#" + _themeColorProvider.GetColor(Element.ThemeColor.Value);
                if (Element.Color?.Value != null)
                    return "#" + Element.Color?.Value;
                return "";
            }
        }

        public override ICssProperty Clone() =>
            new UnderlineCssProperty(_themeColorProvider)
            {
                Element = Element,
                Selector = Selector
            };

    }
}
