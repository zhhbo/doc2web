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
        private ConcurrentDictionary<ICssClass, string> _dynamicClassesUIDs;

        public CssRegistrator(ICssClassFactory classFactory)
        {
            _classFactory = classFactory;
            _classesToRender = new ConcurrentDictionary<string, byte>();
            _dynamicClassesUIDs = new ConcurrentDictionary<ICssClass, string>();
        }

        public string Register(string styleId)
        {
            _classesToRender.TryAdd(styleId, 0);
            return styleId;
        }

        public string Register(ParagraphProperties pPr)
        {
            var cls = _classFactory.Build(pPr);
            return TryGetDynamicClass(cls, "p.{0}");
        }
        public string Register(RunProperties rPr)
        {
            var cls = _classFactory.Build(rPr);
            return TryGetDynamicClass(cls, "span.{0}");
        }

        private string TryGetDynamicClass(ICssClass cls, string selectorPrefix)
        {
            if (cls.IsEmpty) return "";
            string uid = "dyn-" + Guid.NewGuid().ToString().Replace("-", "");
            cls.Selector = String.Format(selectorPrefix, uid);
            if (_dynamicClassesUIDs.TryAdd(cls, uid))
                return uid;
            else if (_dynamicClassesUIDs.TryGetValue(cls, out uid))
                return uid;
            throw new InvalidOperationException();
        }


        public void RenderInto(StringBuilder sb)
        {
            var cssData = new CssData();
            var defaults = _classFactory.BuildDefaults();
            var clsToRender =
                _classesToRender
                    .Keys
                    .Select(_classFactory.Build)
                    .Concat(_dynamicClassesUIDs.Keys)
                    .Concat(defaults)
                    .ToArray();

            foreach (var cls in clsToRender)
                cssData.AddRange(cls.AsCss());

            cssData.RenderInto(sb);
        }
    }
}
