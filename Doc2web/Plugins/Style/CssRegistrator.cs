using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Threading.Tasks;

namespace Doc2web.Plugins.Style
{
    public class CssRegistrator : ICssRegistrator
    {
        private ICssClassFactory _classFactory;
        private object _lockObj;
        private HashSet<string> _classesToRender;

        public CssRegistrator(ICssClassFactory classFactory)
        {
            _lockObj = new object();
            _classFactory = classFactory;
            _classesToRender = new HashSet<string>();
        }

        public string Register(string styleId)
        {
            lock (_lockObj) {
                _classesToRender.Add(styleId);
            }
            return styleId;
        }

        public string Register(ParagraphProperties pPr)
        {
            throw new NotImplementedException();
        }

        public string Register(RunProperties rPr)
        {
            throw new NotImplementedException();
        }

        public void RenderInto(StringBuilder sb)
        {
            Parallel.ForEach(
                _classesToRender,
                () => new CssData(),
                (styleId, state, cssData) =>
                {
                    var cssClass = _classFactory.Build(styleId);
                    cssData.AddRange(cssClass.AsCss());
                    return cssData;
                },
                x => {
                    lock (sb)
                    {
                        x.RenderInto(sb);
                    }
                }
                );
        }
    }
}
