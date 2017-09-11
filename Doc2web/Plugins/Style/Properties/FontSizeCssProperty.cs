using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Plugins.Style.Properties
{
    public class FontSizeCssProperty : CssProperty<FontSize>
    {
        public override void InsertCss(CssData cssData)
        {
            var size = GetSpecificHashcode();
            if (size != -1)
            {
                string points = Math.Round((double)GetSpecificHashcode()/2, 2).ToString();
                cssData.AddAttribute(Selector, "font-size", points + "pt");
            }
        }

        public override short GetSpecificHashcode() => GetSize(Element);


        public override bool HaveSameOuput(FontSize element) =>
            GetSize(element) == GetSpecificHashcode();

        private static short GetSize(FontSize e)
        {
            var val = e.Val?.Value;
            if (val != null && short.TryParse(val, out short result))
                return result;
            return -1;
        }
    }
}
