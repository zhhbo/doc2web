using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Css
{
    public class ParagraphClassFactory
    {
        private StyleConfig _config;
        private ICssPropertiesFactory _propsFac;
        private IStylePropsCache _propsCache;
        private IDefaultsProvider _defaultsProvider;

        public ParagraphClassFactory(
            StyleConfig config,
            IDefaultsProvider defaultsProvider,
            IStylePropsCache propsCache,
            Func<CssPropertySource, ICssPropertiesFactory> _facBuiler)
        {
            _config = config;
            _propsFac = _facBuiler(CssPropertySource.Paragraph);
            _propsCache = propsCache;
            _defaultsProvider = defaultsProvider;
        }

        public CssClass2 Build(ParagraphProperties pPr)
        {
            var cssClass = new CssClass2();
            string styleId = pPr.ParagraphStyleId?.Val;
            var propsInline = BuildInline(pPr);

            if (propsInline.Count == 0 && styleId == null) return null;

            cssClass.Props.AddMany(propsInline);
            if (styleId != null)
            {
                cssClass.Props.AddMany(_propsCache.Get(styleId));
            }

            if (propsInline.Count == 0 && styleId != null)
            {
                cssClass.Name = styleId;
            } else
            {
                cssClass.Name = GenerateDynName();
            }

            cssClass.Props.AddMany(_defaultsProvider.Paragraph);

            return cssClass;
        }

        private CssPropertiesSet BuildInline(ParagraphProperties pPr)
        {
            var propsInline = _propsFac.Build(pPr);
            var set = new CssPropertiesSet();
            set.AddMany(propsInline);
            return set;
        }

        private string GenerateDynName()
        {
            var uid = Guid.NewGuid().ToString().Replace("-", "");
            return _config.DynamicCssClassPrefix + uid;
        }
    }
}
