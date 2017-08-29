using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;

namespace Doc2web.Plugins.Style
{
    public class CssClassFactory : ICssClassFactory
    {
        public CssClassFactory(WordprocessingDocument wpDoc, ICssPropertiesFactory cssPropertyFactory)
        {

        }

        public ICssClass Build(string styleId)
        {
            throw new NotImplementedException();
        }

        public ICssClass Build(ParagraphProperties pPr)
        {
            throw new NotImplementedException();
        }

        public ICssClass Build(RunProperties rPr)
        {
            throw new NotImplementedException();
        }
    }
}
