using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Css
{
    public class CssClass
    {
        public CssPropertiesSet Props { get; set; }

        public string Name { get; set; }

        public CssClass()
        {
            Name = "";
            Props = new CssPropertiesSet();
        }

        public void InsertCss(CssData data)
        {
            Props.Selector = "." + Name;
            Props.InsertCss(data);
        }
    }
}
