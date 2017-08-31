using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Properties
{
    public class BoldCssProperty : BooleanCssProperty<Bold>
    {
        public override void SetOff(CssData data)
        {
            data.AddAttribute(Selector, "font-weight", "normal");
        }

        public override void SetOn(CssData data)
        {
            data.AddAttribute(Selector, "font-weight", "bold");
        }
    }
}
