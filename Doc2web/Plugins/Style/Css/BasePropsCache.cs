using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Linq;

namespace Doc2web.Plugins.Style.Css
{
    public abstract class BasePropsCache : IPropsCache
    {
        private Dictionary<string, CssPropertiesSet> _cache;
        private IEnumerable<DocumentFormat.OpenXml.Wordprocessing.Style> _styles;

        public BasePropsCache(IEnumerable<DocumentFormat.OpenXml.Wordprocessing.Style> styles)
        {
            _cache = new Dictionary<string, CssPropertiesSet>();
            _styles = styles;
        }

        public IReadOnlyDictionary<string, CssPropertiesSet> Cache => _cache;

        public CssPropertiesSet Get(string styleId)
        {
            if (_cache.TryGetValue(styleId, out CssPropertiesSet result))
            {
                return result;
            } else
            {
                lock (_cache)
                {
                    return AddStyle(styleId);
                }
            }
        }

        private CssPropertiesSet AddStyle(string styleId)
        {
            try
            {
                var style = _styles.First(x => x.StyleId?.Value == styleId);
                ICssProperty[] props = BuildProps(style);
                var set = new CssPropertiesSet();
                set.AddMany(props);

                _cache.Add(styleId, set);

                if (style.BasedOn?.Val != null)
                {
                    if (_cache.TryGetValue(
                        style.BasedOn.Val,
                        out CssPropertiesSet basedOn))
                    {
                        set.AddMany(basedOn);
                    }
                    else
                    {
                        set.AddMany(AddStyle(style.BasedOn.Val));
                    }
                }

                return set;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public abstract ICssProperty[] BuildProps(DocumentFormat.OpenXml.Wordprocessing.Style style);
        //{
        //    return _propsFac.Build(style.StyleParagraphProperties);
        //}
    }
}
