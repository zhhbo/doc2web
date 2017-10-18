using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using WStyle = DocumentFormat.OpenXml.Wordprocessing.Style;

namespace Doc2web.Plugins.Style.Css
{
    public class RStyleRPropsCache : BaseStylePropsCache
    {
        private ICssPropertiesFactory _propsFac;

        public RStyleRPropsCache(
            Func<CssPropertySource, ICssPropertiesFactory> _facBuiler,
            IEnumerable<WStyle> styles) : base(styles)
        {
            _propsFac = _facBuiler(CssPropertySource.Run);
        }

        public override CssPropertiesSet BuildProps(WStyle style)
        {
            if (style.StyleRunProperties != null)
                return _propsFac.Build(style.StyleRunProperties);
            return new CssPropertiesSet();
        }

    }
}
