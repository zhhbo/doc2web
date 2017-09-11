using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Properties
{
    public static class Utils
    {
        public static CssData AsCss(this ICssProperty prop)
        {
            var cssData = new CssData();
            prop.InsertCss(cssData);
            return cssData;
        }

        public static CssData AsCss(this ICssClass cls)
        {
            var cssData = new CssData();
            cls.InsertCss(cssData);
            return cssData;
        }
    }
}
