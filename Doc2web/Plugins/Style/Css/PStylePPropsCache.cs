using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using WStyle = DocumentFormat.OpenXml.Wordprocessing.Style;

namespace Doc2web.Plugins.Style.Css
{
    public class PStylePPropsCache : BaseStylePropsCache
    {
        private ICssPropertiesFactory _propsFac;

        public PStylePPropsCache(
            Func<CssPropertySource, ICssPropertiesFactory> _facBuiler,
            IEnumerable<WStyle> styles) : base(styles)
        {
            _propsFac = _facBuiler(CssPropertySource.Paragraph);
        }

        public override ICssProperty[] BuildProps(WStyle style)
        {
            if (style.StyleParagraphProperties != null)
                return _propsFac.Build(style.StyleParagraphProperties);
            return new ICssProperty[0];
        }

    }
}
