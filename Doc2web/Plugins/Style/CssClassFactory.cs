using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using System.Linq;

namespace Doc2web.Plugins.Style
{
    public class CssClassFactory : ICssClassFactory
    {
        private Dictionary<string, DocumentFormat.OpenXml.Wordprocessing.Style> _styleDictionary;
        private StyleConfiguration _config;
        private DocDefaults _docDefaults;
        private Dictionary<string, ParagraphCssClass> _pClassesChache;
        private Dictionary<string, RunCssClass> _rClassesCache;
        private ICssPropertiesFactory _propsFac;


        public CssClassFactory(
            Styles styles, 
            StyleConfiguration config,
            ICssPropertiesFactory cssPropertyFactory)
        {
            InitStyleDictionnary(styles);
            _config = config;
            _docDefaults = styles.DocDefaults;
            _pClassesChache = new Dictionary<string, ParagraphCssClass>();
            _rClassesCache = new Dictionary<string, RunCssClass>();
            _propsFac = cssPropertyFactory;
        }

        private void InitStyleDictionnary(Styles styles)
        {
            _styleDictionary =
                styles
                .Elements<DocumentFormat.OpenXml.Wordprocessing.Style>()
                .Where(x => x.StyleId?.Value != null)
                .ToDictionary(x => x.StyleId.Value);
        }

        public List<ICssClass> BuildDefaults()
        {
            var cls = new List<ICssClass>(2);
            TryAddDefaultParagraph(cls);
            TryAddDefaultRun(cls);
            return cls;
        }

        private void TryAddDefaultParagraph(List<ICssClass> cls)
        {
            var pDefaults =
                _docDefaults
                .ParagraphPropertiesDefault?
                .ParagraphPropertiesBaseStyle;
            if (pDefaults != null)
            {
                var pCls = new ParagraphCssClass { Selector = _config.ParagraphCssClassPrefix };
                pCls.ParagraphProps.AddMany(_propsFac.Build(pDefaults));
                cls.Add(pCls);
            }
        }

        private void TryAddDefaultRun(List<ICssClass> cls)
        {
            var rDefaults = _docDefaults.RunPropertiesDefault?.RunPropertiesBaseStyle;
            if (rDefaults != null)
            {
                var rCls = new RunCssClass { Selector = _config.RunCssClassPrefix };
                rCls.RunProps.AddMany(_propsFac.Build(rDefaults));
                cls.Add(rCls);
            }
        }

        public ICssClass Build(string styleId)
        {
            if (_pClassesChache.TryGetValue(styleId, out ParagraphCssClass pCls)) return pCls;
            else if (_rClassesCache.TryGetValue(styleId, out RunCssClass rCls)) return rCls;
            else if (_styleDictionary.TryGetValue(styleId, out DocumentFormat.OpenXml.Wordprocessing.Style style)) 
                return BuildAndCache(style);

            throw new ArgumentException("Cannot find a style with this id.");
        }

        private ICssClass BuildAndCache(DocumentFormat.OpenXml.Wordprocessing.Style style)
        {
            switch (style?.Type?.Value)
            {
                case StyleValues.Character:
                    return BuildAndCacheRunStyle(style);
                case StyleValues.Paragraph:
                    return BuildAndCacheParagraphStyle(style);
                default: return null;
            }
        }

        private ICssClass BuildAndCacheRunStyle(DocumentFormat.OpenXml.Wordprocessing.Style style)
        {
            var cls = new RunCssClass();

            if (style.StyleRunProperties != null)
                cls.RunProps.AddMany(_propsFac.Build(style.StyleRunProperties));

            if (style.BasedOn?.Val?.Value != null)
                cls.BasedOn = Build(style.BasedOn.Val.Value) as RunCssClass;

            cls.Selector = $"{_config.RunCssClassPrefix}.{style.StyleId.Value}";
            _rClassesCache.Add(style.StyleId.Value, cls);

            return cls;
        }

        private ICssClass BuildAndCacheParagraphStyle(DocumentFormat.OpenXml.Wordprocessing.Style style)
        {
            var cls = new ParagraphCssClass();

            if (style.StyleRunProperties != null)
                cls.RunProps.AddMany(_propsFac.Build(style.StyleRunProperties));

            if (style.StyleParagraphProperties != null)
                cls.ParagraphProps.AddMany(_propsFac.Build(style.StyleParagraphProperties));

            if (style.BasedOn?.Val?.Value != null)
                cls.BasedOn = Build(style.BasedOn.Val.Value) as ParagraphCssClass;

            cls.Selector = $"{_config.ParagraphCssClassPrefix}.{style.StyleId.Value}";
            _pClassesChache.Add(style.StyleId.Value, cls);

            return cls;
        }

        public ICssClass Build(ParagraphProperties pPr)
        {
            var cls = new ParagraphCssClass();
            cls.ParagraphProps.AddMany(_propsFac.Build(pPr));
            return cls;
        }

        public ICssClass Build(RunProperties rPr)
        {
            var cls = new RunCssClass();
            cls.RunProps.AddMany(_propsFac.Build(rPr));
            return cls;
        }
    }
}
