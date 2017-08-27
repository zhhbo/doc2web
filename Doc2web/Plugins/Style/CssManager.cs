using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Plugins.Style
{
    public class CssManager : ICssManager
    {
        private ICssClassFactory _classFactory;

        public CssManager(ICssClassFactory classFactory)
        {
            _classFactory = classFactory;
        }

        public string RegisterPropertiesAsClass(ParagraphProperties pPr)
        {
            throw new NotImplementedException();
        }

        public string RegisterPropertiesAsClass(RunProperties rPr)
        {
            throw new NotImplementedException();
        }

        public string RegisterStyleIdAsClass(string styleId)
        {
            throw new NotImplementedException();
        }
    }
}
