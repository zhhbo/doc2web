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
        private ClsNameGenerator _clsNameGenerator;
        private ICssPropertiesFactory _propsFac;
        private IStylePropsCache _stylePropsCache;
        private INumberingPropsCache _numPropsCache;
        private IDefaultsProvider _defaultsProvider;

        public ParagraphClassFactory(
            StyleConfig config,
            ClsNameGenerator clsNameGenerator,
            IDefaultsProvider defaultsProvider,
            IStylePropsCache stylePropsCache,
            INumberingPropsCache numPropsCache,
            Func<CssPropertySource, ICssPropertiesFactory> _facBuiler)
        {
            _config = config;
            _clsNameGenerator = clsNameGenerator;
            _propsFac = _facBuiler(CssPropertySource.Paragraph);
            _stylePropsCache = stylePropsCache;
            _numPropsCache = numPropsCache;
            _defaultsProvider = defaultsProvider;
        }

        public CssClass Build(ParagraphClassParam param)
        {
            var cssClass = new CssClass();
            cssClass.Props = BuildInline(param.InlineProperties);

            if (param.NumberingId.HasValue)
                AddNumberingStyleProps(param, cssClass);
            else
                AddStyleProps(param.StyleId, cssClass);

            AddDefaultsProps(cssClass);

            cssClass.Name = GenerateDynName();
            return cssClass;
        }

        private void AddDefaultsProps(CssClass cssClass)
        {
            AddOrSet(cssClass, _defaultsProvider.Paragraph);
        }

        private void AddNumberingStyleProps(ParagraphClassParam param, CssClass cssClass)
        {
            if (!param.NumberingId.HasValue || !param.NumberingLevel.HasValue) return;
            AddOrSet(
                cssClass,
                _numPropsCache.Get(
                    param.NumberingId.Value,
                    param.NumberingLevel.Value));
        }

        private void AddStyleProps(string styleId, CssClass cssClass)
        {
            if (styleId != null)
            {
                AddOrSet(cssClass, _stylePropsCache.Get(styleId));
                return;
            }
            if (_defaultsProvider.DefaultParagraphStyle != null)
            {
                var props = _stylePropsCache.Get(_defaultsProvider.DefaultParagraphStyle);
                AddOrSet(cssClass, props);
            }
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
            return _clsNameGenerator.GenId();
        }

        private void AddOrSet(CssClass cssClass, CssPropertiesSet props)
        {
            if (cssClass.Props.Count > 0)
                cssClass.Props.AddMany(props);
            else
                cssClass.Props = props;
        }
    }
}
