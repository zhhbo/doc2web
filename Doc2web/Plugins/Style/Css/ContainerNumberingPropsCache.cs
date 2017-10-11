using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Plugins.Style.Css
{
    public class ContainerNumberingPropsCache : BaseNumberingPropsCache
    {
        private ICssPropertiesFactory _propsFac;

        public ContainerNumberingPropsCache(
            Func<CssPropertySource, ICssPropertiesFactory> facBuilder,
            INumberingProvider numberingCrawler) : base(numberingCrawler)
        {
            _propsFac = facBuilder(CssPropertySource.Paragraph);
        }

        public override CssPropertiesSet BuildPropertiesSet(Level arg)
        {
            if (arg.PreviousParagraphProperties == null)
                return new CssPropertiesSet();

            var set = new CssPropertiesSet();
            set.AddMany(_propsFac.Build(arg.PreviousParagraphProperties));
            return set;
        }
    }
}
