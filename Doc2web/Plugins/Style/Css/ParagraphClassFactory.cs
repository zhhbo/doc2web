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

        public CssClass2 Build(ParagraphClassParam param)
        {
            var cssClass = new CssClass2();
            var propsInline = BuildInline(param.InlineProperties);

            if (WillBeEmtpyClass(param, propsInline))
                return null;

            cssClass.Props.AddMany(propsInline);
            AddStyleProps(param.StyleId, cssClass);
            AddNumberingStyleProps(param, cssClass);
            SetClassName(param, cssClass, propsInline);
            AddDefaultsProps(cssClass);

            return cssClass;
        }

        private void SetClassName(ParagraphClassParam param, CssClass2 cssClass, CssPropertiesSet propsInline)
        {
            if (propsInline.Count == 0 && param.StyleId != null)
                cssClass.Name = param.StyleId;
            else
                cssClass.Name = GenerateDynName();
        }

        private void AddDefaultsProps(CssClass2 cssClass)
        {
            cssClass.Props.AddMany(_defaultsProvider.Paragraph);
        }

        private void AddNumberingStyleProps(ParagraphClassParam param, CssClass2 cssClass)
        {
            if (!param.NumberingId.HasValue || !param.NumberingLevel.HasValue) return;
            cssClass.Props.AddMany(
                _numPropsCache.Get(
                    param.NumberingId.Value,
                    param.NumberingLevel.Value));
        }

        private void AddStyleProps(string styleId, CssClass2 cssClass)
        {
            if (styleId == null) return;
            cssClass.Props.AddMany(_stylePropsCache.Get(styleId));
        }

        private static bool WillBeEmtpyClass(ParagraphClassParam param, CssPropertiesSet propsInline) => 
            propsInline.Count == 0 &&
            param.StyleId == null &&
            (!param.NumberingId.HasValue || !param.NumberingLevel.HasValue);

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
