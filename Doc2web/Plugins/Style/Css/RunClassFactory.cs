using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Linq;

namespace Doc2web.Plugins.Style.Css
{
    public class RunClassFactory
    {
        private StyleConfig _config;
        private IDefaultsProvider _defaults;
        private IPropsCache _pStylePropsCache;
        private IPropsCache _rStylePropsCache;
        private ICssPropertiesFactory _propsFac;

        public RunClassFactory(
            StyleConfig config,
            IDefaultsProvider defaults,
            IPropsCache pStylePropsCache,
            IPropsCache rStylePropsCache,
            Func<CssPropertySource, ICssPropertiesFactory> factoryBuilder)
        {
            _config = config;
            _defaults = defaults;
            _pStylePropsCache = pStylePropsCache;
            _rStylePropsCache = rStylePropsCache;
            _propsFac = factoryBuilder(CssPropertySource.Run);
        }

        public CssClass2 Build(
            string pStyleId,
            RunProperties rProps)
        {
            var cssClass = new CssClass2();
            string styleId = rProps.RunStyle?.Val;
            var inline = BuildInline(rProps);

            cssClass.Props.AddMany(inline);

            if (styleId != null)
                cssClass.Props.AddMany(_rStylePropsCache.Get(styleId));

            if (pStyleId != null)
                cssClass.Props.AddMany(_pStylePropsCache.Get(pStyleId));

            if (inline.Count == 0 && styleId != null && pStyleId == null)
            {
                cssClass.Name = styleId;
            } else
            {
                cssClass.Name = GenerageDynamicName();
            }

            if (cssClass.Props.Any())
                cssClass.Props.AddMany(_defaults.Run);

            return cssClass;
        }

        private string GenerageDynamicName() =>
            _config.DynamicCssClassPrefix +
            Guid.NewGuid().ToString().Replace("-", "");

        private CssPropertiesSet BuildInline(RunProperties rProps)
        {
            var props = _propsFac.Build(rProps);
            var set = new CssPropertiesSet();
            set.AddMany(props);
            return set;
        }
    }
}
