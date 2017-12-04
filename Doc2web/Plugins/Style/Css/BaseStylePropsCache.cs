using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Linq;

namespace Doc2web.Plugins.Style.Css
{
    public abstract class BaseStylePropsCache : IStylePropsCache
    {
        private Dictionary<string, CssPropertiesSet> _cache;
        private IEnumerable<DocumentFormat.OpenXml.Wordprocessing.Style> _styles;

        public BaseStylePropsCache(IEnumerable<DocumentFormat.OpenXml.Wordprocessing.Style> styles)
        {
            _cache = new Dictionary<string, CssPropertiesSet>();
            _styles = styles;
        }

        public IReadOnlyDictionary<string, CssPropertiesSet> Cache => _cache;

        public CssPropertiesSet Get(string styleId)
        {
            lock (_cache)
            {
                if (_cache.TryGetValue(styleId, out CssPropertiesSet result))
                    return result.Clone();

                return AddStyle(styleId).Clone();
            }
        }

        private CssPropertiesSet AddStyle(string styleId)
        {
            var style = _styles.FirstOrDefault(x => x.StyleId?.Value == styleId);
            if (style == null) return new CssPropertiesSet();

            var props = BuildProps(style);
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

        public abstract CssPropertiesSet BuildProps(DocumentFormat.OpenXml.Wordprocessing.Style style);
    }
}
