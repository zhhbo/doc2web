using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Properties
{
    public class ItalicCssProperty : BooleanCssProperty<Italic>
    {
        public override void SetOff(CssData data)
        {
            data.AddAttribute(Selector, "font-style", "normal");
        }

        public override void SetOn(CssData data)
        {
            data.AddAttribute(Selector, "font-style", "italic");
        }
    }
}
