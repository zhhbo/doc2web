﻿using System;
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
            if (_cache.TryGetValue(styleId, out CssPropertiesSet result))
            {
                return result.Clone();
            } else
            {
                lock (_cache)
                {
                    return AddStyle(styleId).Clone();
                }
            }
        }

        private CssPropertiesSet AddStyle(string styleId)
        {
            try
            {
                var style = _styles.First(x => x.StyleId?.Value == styleId);
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
            catch
            {
                return new CssPropertiesSet();
            }
        }

        public abstract CssPropertiesSet BuildProps(DocumentFormat.OpenXml.Wordprocessing.Style style);
    }
}
