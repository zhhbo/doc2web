using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Concurrent;

namespace Doc2web.Plugins.Style
{
    public class CssRegistrator : ICssRegistrator
    {
        private ICssClassFactory _classFactory;
        private ConcurrentDictionary<string, byte> _classesToRender;
        private Dictionary<ICssClass, string> _dynamicParagraphClassesUIDs;
        private Dictionary<ICssClass, string> _dynamicRunClassesUIDs;

        public CssRegistrator(ICssClassFactory classFactory)
        {
            _classFactory = classFactory;
            _classesToRender = new ConcurrentDictionary<string, byte>();
            _dynamicParagraphClassesUIDs = new Dictionary<ICssClass, string>();
            _dynamicRunClassesUIDs = new Dictionary<ICssClass, string>();
        }

        public string Register(string styleId)
        {
            _classesToRender.TryAdd(styleId, 0);
            return styleId;
        }

        public string Register(ParagraphProperties pPr)
        {
            var cls = _classFactory.Build(pPr);
            return TryGetDynamicClass(cls, "p.{0}", _dynamicParagraphClassesUIDs);
        }
        public string Register(RunProperties rPr)
        {
            var cls = _classFactory.Build(rPr);
            return TryGetDynamicClass(cls, "span.{0}",_dynamicRunClassesUIDs);
        }

        private string TryGetDynamicClass(ICssClass cls, string selectorPrefix, Dictionary<ICssClass, string> dict)
        {
            if (cls.IsEmpty) return "";
            if (dict.TryGetValue(cls, out string uid)) return uid;

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

            cls.Selector = String.Format(selectorPrefix, uid);
            return uid;
        }


        public void RenderInto(StringBuilder sb)
        {
            var cssData = new CssData();
            var defaults = _classFactory.BuildDefaults();
            var clsToRender =
                _classesToRender
                    .Keys
                    .Select(_classFactory.Build)
                    .Concat(_dynamicParagraphClassesUIDs.Keys)
                    .Concat(_dynamicRunClassesUIDs.Keys)
                    .Concat(defaults)
                    .ToArray();

            foreach (var cls in clsToRender)
                cssData.AddRange(cls.AsCss());

            cssData.RenderInto(sb);
        }
    }
}
