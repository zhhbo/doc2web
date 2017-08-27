using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Properties
{
    public class BoldCssProperty : BaseCssProperty<Bold>
    {
        protected bool? ExplicitBold
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
            var data = new CssData();
            if (ExplicitBold.HasValue)
            {
                if (ExplicitBold.Value)
                    data.AddAttribute(Selector, "font-weight", "bold");
                else
                    data.AddAttribute(Selector, "font-weight", "normal");
            } else
            {
                data.AddAttribute(Selector, "font-weight", "bold");
            }
            return data;
        }

        public override int CompareTo(Bold element)
        {
            var other = new BoldCssProperty { Element = element };
            return (ExplicitBold == other.ExplicitBold) ? 0 : -1;
        }
    }
}
