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
        private IStylePropsCache _rStylePropsCache;
        private ICssPropertiesFactory _propsFac;

        public RunClassFactory(
            StyleConfig config,
            IDefaultsProvider defaults,
            IStylePropsCache pStylePropsCache,
            IStylePropsCache rStylePropsCache,
            Func<CssPropertySource, ICssPropertiesFactory> factoryBuilder)
        {
            _config = config;
            _defaults = defaults;
            _pStylePropsCache = pStylePropsCache;
            _rStylePropsCache = rStylePropsCache;
            _propsFac = factoryBuilder(CssPropertySource.Run);
        }

        public CssClass2 Build(RunClassParam param)
        {
            var cssClass = new CssClass2();
            string styleId = param.RunStyleId;
            var inline = BuildInline(param.InlineProps);

            cssClass.Props.AddMany(inline);

            if (styleId != null)
                cssClass.Props.AddMany(_rStylePropsCache.Get(styleId));

            if (param.ParagraphStyleId != null)
                cssClass.Props.AddMany(_pStylePropsCache.Get(param.ParagraphStyleId));

            if (inline.Count == 0 && styleId != null && param.ParagraphStyleId == null)
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

        private CssPropertiesSet BuildInline(OpenXmlElement rProps)
        {
            var props = _propsFac.Build(rProps);
            var set = new CssPropertiesSet();
            set.AddMany(props);
            return set;
        }
    }
}
