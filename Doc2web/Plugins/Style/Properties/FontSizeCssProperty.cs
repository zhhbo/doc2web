using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Plugins.Style.Properties
{
    [RunCssProperty(typeof(FontSize))]
    public class FontSizeCssProperty : HaflPointCssProperty<FontSize>
    {
        /// <summary>
        /// Only used as a commodity during testing.
        /// </summary>
        public override void InsertCss(CssData cssData)
        {
            InsertCss(new CssPropertiesSet(), cssData);
        }

        public override void InsertCss(CssPropertiesSet set, CssData cssData)
        {
            if (Val == -1) return;

            if (set.Get<FontSizeCSCssProperty>() != null &&
                Utils.ComplexScriptApplies(set)) return;

            string points = Math.Round(Val, 2).ToString();
            cssData.AddAttribute(Selector, "font-size", points + "pt");
        }
    }
}
