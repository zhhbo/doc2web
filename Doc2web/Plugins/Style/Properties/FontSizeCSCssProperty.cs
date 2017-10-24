using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Properties
{
    [RunCssProperty(typeof(FontSizeComplexScript))]
    public class FontSizeCSCssProperty : HaflPointCssProperty<FontSizeComplexScript>
    {
        public override void InsertCss(CssPropertiesSet others, CssData cssData)
        {
            if (Utils.ComplexScriptApplies(others))
                cssData.AddAttribute(Selector, "font-size", Val + "pt");
        }
    }
}
