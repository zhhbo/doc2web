using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Css
{
    public class ParagraphClassFactory : IParagraphClassFactory
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
            cssClass.Props = BuildInline(param.InlineProperties);

            AddStyleProps(param.StyleId, cssClass);
            AddNumberingStyleProps(param, cssClass);
            AddDefaultsProps(cssClass);

            cssClass.Name = GenerateDynName();
            return cssClass;
        }

        private void AddDefaultsProps(CssClass2 cssClass)
        {
            AddOrSet(cssClass, _defaultsProvider.Paragraph);
        }

        private void AddNumberingStyleProps(ParagraphClassParam param, CssClass2 cssClass)
        {
            if (!param.NumberingId.HasValue || !param.NumberingLevel.HasValue) return;
            AddOrSet(
                cssClass,
                _numPropsCache.Get(
                    param.NumberingId.Value,
                    param.NumberingLevel.Value));
        }

        private void AddStyleProps(string styleId, CssClass2 cssClass)
        {
            if (styleId == null) return;
            AddOrSet(cssClass, _stylePropsCache.Get(styleId));
        }

        private static bool WillBeEmtpyClass(ParagraphClassParam param, CssPropertiesSet propsInline) => 
            propsInline.Count == 0 &&
            param.StyleId == null &&
            (!param.NumberingId.HasValue || !param.NumberingLevel.HasValue);

        private CssPropertiesSet BuildInline(OpenXmlElement pPr)
        {
            if (pPr == null) return new CssPropertiesSet();
            return _propsFac.Build(pPr);
        }

        private string GenerateDynName()
        {
            var uid = Guid.NewGuid().ToString().Replace("-", "");
            return _config.DynamicCssClassPrefix + uid;
        }

        private void AddOrSet(CssClass2 cssClass, CssPropertiesSet props)
        {
            if (cssClass.Props.Count > 0)
                cssClass.Props.AddMany(props);
            else
                cssClass.Props = props;
        }
    }
}
