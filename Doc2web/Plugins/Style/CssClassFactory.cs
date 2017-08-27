using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Plugins.Style
{
    public class CssClassFactory : ICssClassFactory
    {
        public CssClassFactory(ICssPropertiesFactory cssPropertyFactory)
        {

        }

        public CssClass Build(string styleId)
        {
            throw new NotImplementedException();
        }

        public CssClass Build(ParagraphProperties pPr)
        {
            throw new NotImplementedException();
        }

        public CssClass Build(RunProperties rPr)
        {
            throw new NotImplementedException();
        }
    }
}
