using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Properties
{
    public class UnderlineCssProperty : BaseCssProperty<Underline>
    {
        private IThemeColorProvider _themeColorProvider;

        public UnderlineCssProperty(IThemeColorProvider themeColorProvider) : base()
        {
            _themeColorProvider = themeColorProvider;
        }

        public override CssData AsCss()
        {
            var data = new CssData();
            data.AddAttribute(
                Selector,
                "text-decoration",
                $"underline {Style} {Color}".TrimEnd());
            return data;
        }

        public override short GetSpecificHashcode() => 
            (short)(Element.Val?.Value, Color).GetHashCode();

        public override bool HaveSameOuput(Underline element)
        {
            var other = new UnderlineCssProperty(_themeColorProvider) { Element = element };
            return other.Style == Style && other.Color == Color;
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
    }
}
