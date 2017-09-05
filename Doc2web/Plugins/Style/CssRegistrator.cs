using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Threading.Tasks;
using System.Linq;

namespace Doc2web.Plugins.Style
{
    public class CssRegistrator : ICssRegistrator
    {
        private ICssClassFactory _classFactory;
        private object _lockObj;
        private HashSet<string> _classesToRender;
        private Dictionary<ICssClass, string> _dynamicClassesUIDs;

        public CssRegistrator(ICssClassFactory classFactory)
        {
            _lockObj = new object();
            _classFactory = classFactory;
            _classesToRender = new HashSet<string>();
            _dynamicClassesUIDs = new Dictionary<ICssClass, string>();
        }

        public string Register(string styleId)
        {
            lock (_lockObj)
            {
                _classesToRender.Add(styleId);
            }
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
            lock (_dynamicClassesUIDs)
            {
                if (_dynamicClassesUIDs.TryGetValue(cls, out string uid))
                    return uid;

                uid = "dyn-" + Guid.NewGuid().ToString().Replace("-", "");
                cls.Selector = String.Format(selectorPrefix, uid);
                _dynamicClassesUIDs.Add(cls, uid);
                return uid;
            }
        }


        public void RenderInto(StringBuilder sb)
        {
            var cssData = new CssData();
            var defaults = _classFactory.BuildDefaults();
            var clsToRender =
                _classesToRender
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
