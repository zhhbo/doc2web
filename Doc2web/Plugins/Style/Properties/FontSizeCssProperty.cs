using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Plugins.Style.Properties
{
    [RunCssProperty(typeof(FontSize))]
    public class FontSizeCssProperty : CssProperty<FontSize>
    {
        public override void InsertCss(CssData cssData)
        {
            var size = GetHashCode();
            if (size != -1)
            {
                string points = Math.Round((double)size / 2, 2).ToString();
                cssData.AddAttribute(Selector, "font-size", points + "pt");
            }
        }

        public override int GetHashCode() => GetSize(Element);

        public override bool Equals(ICssProperty element)
        {
            if (element is FontSizeCssProperty other)
                return other.GetHashCode() == GetHashCode();
            return false;
        }

        private static short GetSize(FontSize e)
        {
            var val = e.Val?.Value;
            if (val != null && short.TryParse(val, out short result))
                return result;
            return -1;
        }
    }
}
