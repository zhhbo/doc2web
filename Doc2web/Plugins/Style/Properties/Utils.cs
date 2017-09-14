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

        public static double TwipsToCm(int twips)
        {
            return twips / 567.0;
        }

        public static double TwipsToPageRatio(int twips)
        {
            return twips / (567.0 * 21.59);
        }
    }
}
