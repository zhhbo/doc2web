using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Properties
{
    public class SmallCapsCssProperty : BooleanCssProperty<SmallCaps>
    {
        public override void SetOff(CssData data)
        {
            data.AddAttribute(Selector, "font-variant", "normal");
        }

        public override void SetOn(CssData data)
        {
            data.AddAttribute(Selector, "font-variant", "small-caps");
        }
    }
}
