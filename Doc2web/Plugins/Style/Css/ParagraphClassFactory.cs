using DocumentFormat.OpenXml;
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
        private IStylePropsCache _stylePropsCache;
        private INumberingPropsCache _numPropsCache;
        private IDefaultsProvider _defaultsProvider;

        public ParagraphClassFactory(
            StyleConfig config,
            IDefaultsProvider defaultsProvider,
            IStylePropsCache stylePropsCache,
            INumberingPropsCache numPropsCache,
            Func<CssPropertySource, ICssPropertiesFactory> _facBuiler)
        {
            _config = config;
            _propsFac = _facBuiler(CssPropertySource.Paragraph);
            _stylePropsCache = stylePropsCache;
            _numPropsCache = numPropsCache;
            _defaultsProvider = defaultsProvider;
        }

        public CssClass2 Build(ParagraphClassParams param)
        {
            var cssClass = new CssClass2();
            var propsInline = BuildInline(param.InlineProperties);

            if (propsInline.Count == 0 && 
                param.StyleId == null &&
                (!param.NumberingId.HasValue || !param.NumberingLevel.HasValue))
                return null;

            cssClass.Props.AddMany(propsInline);
            if (param.StyleId != null)
            {
                cssClass.Props.AddMany(_stylePropsCache.Get(param.StyleId));
            }

            if (param.NumberingId.HasValue && param.NumberingLevel.HasValue)
                cssClass.Props.AddMany(
                    _numPropsCache.Get(
                        param.NumberingId.Value,
                        param.NumberingLevel.Value));

            if (propsInline.Count == 0 && param.StyleId != null)
            {
                cssClass.Name = param.StyleId;
            } else
            {
                cssClass.Name = GenerateDynName();
            }

            cssClass.Props.AddMany(_defaultsProvider.Paragraph);

            return cssClass;
        }

        private CssPropertiesSet BuildInline(OpenXmlElement pPr)
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
