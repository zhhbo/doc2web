using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Linq;
using DocumentFormat.OpenXml;

namespace Doc2web.Plugins.Style.Css
{
    public class RunClassFactory : IRunClassFactory
    {
        private StyleConfig _config;
        private ClsNameGenerator _clsNameGenerator;
        private IDefaultsProvider _defaults;
        private IStylePropsCache _pStylePropsCache;
        private INumberingPropsCache _numPropsCache;
        private IStylePropsCache _rStylePropsCache;
        private ICssPropertiesFactory _propsFac;

        public RunClassFactory(
            StyleConfig config,
            ClsNameGenerator clsNameGenerator,
            IDefaultsProvider defaults,
            IStylePropsCache pStylePropsCache,
            INumberingPropsCache numPropsCache,
            IStylePropsCache rStylePropsCache,
            Func<CssPropertySource, ICssPropertiesFactory> factoryBuilder)
        {
            _config = config;
            _clsNameGenerator = clsNameGenerator;
            _defaults = defaults;
            _pStylePropsCache = pStylePropsCache;
            _numPropsCache = numPropsCache;
            _rStylePropsCache = rStylePropsCache;
            _propsFac = factoryBuilder(CssPropertySource.Run);
        }

        public CssClass Build(RunClassParam param)
        {
            var cssClass = new CssClass { Name = GenerageDynamicName() };
            var inline = BuildInline(param.InlineProperties);

            // This is according to the doc
            // cssClass.Props.AddMany(inline);
            // AddRunStyle(cssClass, param.RunStyleId);
            // AddNumberingProps(cssClass, param);

            // This is according to me
            AddNumberingProps(cssClass, param);
            cssClass.Props.AddMany(inline);
            AddRunStyle(cssClass, param.RunStyleId);
            AddParagraphStyle(cssClass, param.ParagraphStyleId);

            AddDefaults(cssClass);
            return cssClass;
        }

        private void AddDefaults(CssClass cssClass)
        {
            cssClass.Props.AddMany(_defaults.Run);
        }

        private void AddNumberingProps(CssClass cssClass, RunClassParam param)
        {
            if (!param.NumberingId.HasValue || !param.NumberingLevel.HasValue) return;
            cssClass.Props.AddMany(_numPropsCache.Get(param.NumberingId.Value, param.NumberingLevel.Value));
        }

        private void AddParagraphStyle(CssClass cssClass, string styleId)
        {
            if (styleId != null)
            {
                // cssClass.Props.AddMany(_pStylePropsCache.Get(styleId));
                return;
            }
            if (_defaults.DefaultRunStyle != null)
            {
                var props = _rStylePropsCache.Get(_defaults.DefaultRunStyle);
                cssClass.Props.AddMany(props);
            }
        }

        private void AddRunStyle(CssClass cssClass, string styleId)
        {
            if (styleId == null) return;
            cssClass.Props.AddMany(_rStylePropsCache.Get(styleId));
        }

        private static bool WillBeEmptyClass(RunClassParam param, CssPropertiesSet inline) => 
            inline.Count == 0 &&
            param.RunStyleId == null &&
            param.ParagraphStyleId == null &&
            (!param.NumberingId.HasValue || !param.NumberingLevel.HasValue);

        private string GenerageDynamicName() => _clsNameGenerator.GenId();

        private CssPropertiesSet BuildInline(OpenXmlElement rProps)
        {
            var set = new CssPropertiesSet();
            if (rProps == null) return set;
            var props = _propsFac.Build(rProps);
            set.AddMany(props);
            return set;
        }
    }
}
