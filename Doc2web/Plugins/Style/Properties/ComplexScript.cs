using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Properties
{
    [RunCssProperty(typeof(ComplexScript))]
    public class ComplexScriptCssProperty : BooleanCssProperty<ComplexScript>
    {
        public override void SetOff(CssData data) { }

        public override void SetOn(CssData data) { }
    }
}
