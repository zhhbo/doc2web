using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Css
{
    public class CssClass2
    {
        public CssPropertiesSet Props { get; set; }

        public string Name { get; set; }

        public string Selector { get; set; }

        public CssClass2()
        {
            Props = new CssPropertiesSet();
        }

        public void InsertCss(CssData data)
        {
            Props.Selector = Selector;
            Props.InsertCss(data);
        }
    }
}
