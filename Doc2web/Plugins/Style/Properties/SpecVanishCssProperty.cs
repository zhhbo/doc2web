using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Properties
{
    public class SpecVanishCssProperty : BooleanCssProperty<SpecVanish>
    {
        public override void SetOff(CssData data) { }

        public override void SetOn(CssData data)
        {
            data.AddAttribute(Selector, "display", "none");
        }
    }
}
