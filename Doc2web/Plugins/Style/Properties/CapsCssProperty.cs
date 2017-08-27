using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Properties
{
    public class CapsCssProperty : BaseCssProperty<Caps>
    {
        private bool? ExpliciteValue
        {
            get
            {
                if (Element.Val != null && Element.Val.HasValue)
                    return Element.Val.Value;
                return null;
            }
        }
        public override CssData AsCss()
        {
            var cssData = new CssData();

            if (ExpliciteValue.HasValue && !ExpliciteValue.Value)
                cssData.AddAttribute(Selector, "text-transform", "lowercase");
            else
                cssData.AddAttribute(Selector, "text-transform", "uppercase");

            return cssData;
        }

        public override int CompareTo(Caps element)
        {
            throw new NotImplementedException();
        }
    }
}
