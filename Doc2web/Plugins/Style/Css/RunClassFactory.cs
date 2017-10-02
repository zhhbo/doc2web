using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Linq;
using DocumentFormat.OpenXml;

namespace Doc2web.Plugins.Style.Css
{
    public class RunClassFactory
    {
        private StyleConfig _config;
        private IDefaultsProvider _defaults;
        private IStylePropsCache _pStylePropsCache;
        private INumberingPropsCache _numPropsCache;
        private IStylePropsCache _rStylePropsCache;
        private ICssPropertiesFactory _propsFac;

        public RunClassFactory(
            StyleConfig config,
            IDefaultsProvider defaults,
            IStylePropsCache pStylePropsCache,
            INumberingPropsCache numPropsCache,
            IStylePropsCache rStylePropsCache,
            Func<CssPropertySource, ICssPropertiesFactory> factoryBuilder)
        {
            _config = config;
            _defaults = defaults;
            _pStylePropsCache = pStylePropsCache;
            _numPropsCache = numPropsCache;
            _rStylePropsCache = rStylePropsCache;
            _propsFac = factoryBuilder(CssPropertySource.Run);
        }

        public CssClass2 Build(RunClassParam param)
        {
            var cssClass = new CssClass2();
            var inline = BuildInline(param.InlineProperties);

            if (WillBeEmptyClass(param, inline))
                return null;

            cssClass.Props.AddMany(inline);

            AddRunStyle(cssClass, param.RunStyleId);

            AddNumberingProps(cssClass, param);

            AddParagraphStyle(cssClass, param.ParagraphStyleId);


            // some work left on the naming part...
            if (inline.Count == 0 && param.RunStyleId != null && param.ParagraphStyleId == null)
            {
                cssClass.Name = param.RunStyleId;
            }
            else
            {
                cssClass.Name = GenerageDynamicName();
            }

            AddDefaults(cssClass);

            return cssClass;
        }

        private void AddDefaults(CssClass2 cssClass)
        {
            if (cssClass.Props.Any())
                cssClass.Props.AddMany(_defaults.Run);
        }

        private void AddNumberingProps(CssClass2 cssClass, RunClassParam param)
        {
            if (!param.NumberingId.HasValue || !param.NumberingLevel.HasValue) return;
            cssClass.Props.AddMany(_numPropsCache.Get(param.NumberingId.Value, param.NumberingLevel.Value));
        }

        private void AddParagraphStyle(CssClass2 cssClass, string styleId)
        {
            if (styleId == null) return;
            cssClass.Props.AddMany(_pStylePropsCache.Get(styleId));
        }

        private void AddRunStyle(CssClass2 cssClass, string styleId)
        {
            if (styleId == null) return;
            cssClass.Props.AddMany(_rStylePropsCache.Get(styleId));
        }

        private static bool WillBeEmptyClass(RunClassParam param, CssPropertiesSet inline) => 
            inline.Count == 0 &&
            param.RunStyleId == null &&
            param.ParagraphStyleId == null &&
            (!param.NumberingId.HasValue || !param.NumberingLevel.HasValue);

        private string GenerageDynamicName() =>
            _config.DynamicCssClassPrefix +
            Guid.NewGuid().ToString().Replace("-", "");

        private CssPropertiesSet BuildInline(OpenXmlElement rProps)
        {
            var props = _propsFac.Build(rProps);
            var set = new CssPropertiesSet();
            set.AddMany(props);
            return set;
        }
    }
}
