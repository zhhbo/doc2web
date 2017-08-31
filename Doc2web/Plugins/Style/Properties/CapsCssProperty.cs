using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Properties
{
    public class CapsCssProperty : BooleanCssProperty<Caps>
    {
        public override void SetOff(CssData data)
        {
            data.AddAttribute(Selector, "text-transform", "lowercase");
        }

        public override void SetOn(CssData data)
        {
            data.AddAttribute(Selector, "text-transform", "uppercase");
        }
    }
}
