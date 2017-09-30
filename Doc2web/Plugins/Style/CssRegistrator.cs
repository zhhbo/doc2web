using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Concurrent;
using DocumentFormat.OpenXml;

namespace Doc2web.Plugins.Style
{
    public class CssRegistrator : ICssRegistrator
    {
        private StyleConfig _config;
        private ICssClassFactory _classFactory;
        private ConcurrentDictionary<string, byte> _stylesToRender;
        private ConcurrentDictionary<(int, int), byte> _numberingToRender;
        private Dictionary<ICssClass, string> _dynamicParagraphClassesUIDs;
        private Dictionary<ICssClass, string> _dynamicRunClassesUIDs;

        public CssRegistrator(
            StyleConfig config,
            ICssClassFactory classFactory)
        {
            _config = config;
            _classFactory = classFactory;
            _stylesToRender = new ConcurrentDictionary<string, byte>();
            _numberingToRender = new ConcurrentDictionary<(int, int), byte>();
            _dynamicParagraphClassesUIDs = new Dictionary<ICssClass, string>();
            _dynamicRunClassesUIDs = new Dictionary<ICssClass, string>();
        }

        public string RegisterStyle(string styleId)
        {
            _stylesToRender.TryAdd(styleId, 0);
            return styleId;
        }

        public string RegisterNumbering(int numberingInstance, int level)
        {
            _numberingToRender.TryAdd((numberingInstance, level), 0);
            return $"numbering-{numberingInstance}-{level}";
        }

        public string[] RegisterParagraphProperties(OpenXmlElement pPr)
        {
            var cls = _classFactory.BuildFromParagraphProperties(pPr);
            return TryGetDynamicClass(cls, _config.ParagraphCssClassPrefix, _dynamicParagraphClassesUIDs);
        }

        public string[] RegisterRunProperties(OpenXmlElement rPr)
        {
            var cls = _classFactory.BuildFromRunProperties(rPr);
            return TryGetDynamicClass(cls, _config.RunCssClassPrefix, _dynamicRunClassesUIDs);
        }

        private string[] TryGetDynamicClass(ICssClass cls, string selectorPrefix, Dictionary<ICssClass, string> dict)
        {
            if (cls.IsEmpty) return new string[] { };
            if (dict.TryGetValue(cls, out string uid))
            {
                return new string[]
                {
                    _config.DynamicCssClassPrefix,
                    uid
                };
            }

            uid = "dyn-" + Guid.NewGuid().ToString().Replace("-", "");

            try
            {
                lock (dict)
                {
                    dict.Add(cls, uid);
                }
            } catch
            {
                uid = dict[cls];
            }

            cls.Selector = $"{selectorPrefix}.{_config.DynamicCssClassPrefix}.{uid}";
            return new string[] { _config.DynamicCssClassPrefix, uid };
        }


        public void RenderInto(StringBuilder sb)
        {
            var cssData = new CssData();
            var numbering =
                _numberingToRender.Keys
                .Select(x => _classFactory.BuildFromNumbering(x.Item1, x.Item2))
                .ToArray();
            var clsToRender =
                _stylesToRender
                    .Keys
                    .Select(_classFactory.BuildFromStyle)
                    .Concat(numbering)
                    .Concat(_dynamicParagraphClassesUIDs.Keys)
                    .Concat(_dynamicRunClassesUIDs.Keys)
                    .Concat(_classFactory.Defaults)
                    .ToArray();

            foreach (var cls in clsToRender)
                cls.InsertCss(cssData);

            cssData.RenderInto(sb);
        }
    }
}
