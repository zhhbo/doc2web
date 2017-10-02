using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Plugins.Style.Css
{
    public abstract class BaseNumberingPropsCache : INumberingPropsCache
    {
        private INumberingProvider _numberingCrawer;
        private Dictionary<(int, int), CssPropertiesSet> _cache;

        public BaseNumberingPropsCache(INumberingProvider numberingCrawler)
        {
            _numberingCrawer = numberingCrawler;
            _cache = new Dictionary<(int, int), CssPropertiesSet>();
        }

        public IReadOnlyDictionary<(int, int), CssPropertiesSet> Cache => _cache;

        public CssPropertiesSet Get(int numberingId, int levelIndex) 
        {
            if (_cache.TryGetValue((numberingId, levelIndex), out CssPropertiesSet cached)) {
                return cached.Clone();
            }

            var newEntry = 
                _numberingCrawer
                .Collect(numberingId, levelIndex)
                .Select(BuildPropertiesSet)
                .Reverse()
                .Aggregate(Reduce);

            lock (_cache)
            {
                _cache.Add((numberingId, levelIndex), newEntry);
            }

            return newEntry.Clone();
        }

        private CssPropertiesSet Reduce(CssPropertiesSet arg1, CssPropertiesSet arg2)
        {
            arg1.AddMany(arg2);
            return arg1;
        }

        public abstract CssPropertiesSet BuildPropertiesSet(Level arg);

        public CssPropertiesSet GetNumber(int numberingId, int levelIndex)
        {
            throw new NotImplementedException();
        }
    }
}
