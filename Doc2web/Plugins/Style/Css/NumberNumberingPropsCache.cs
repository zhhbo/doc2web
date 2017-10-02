using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Plugins.Style.Css
{
    public class NumberNumberingPropsCache : BaseNumberingPropsCache
    {
        private ICssPropertiesFactory _propsFac;

        public NumberNumberingPropsCache(
            Func<CssPropertySource, ICssPropertiesFactory> facBuilder,
            INumberingProvider numberingCrawler) : base(numberingCrawler)
        {
            _propsFac = facBuilder(CssPropertySource.Run);
        }

        public override CssPropertiesSet BuildPropertiesSet(Level arg)
        {
            if (arg.NumberingSymbolRunProperties == null)
                return new CssPropertiesSet();

            var set = new CssPropertiesSet();
            set.AddMany(_propsFac.Build(arg.NumberingSymbolRunProperties));
            return set;
        }
    }
}
