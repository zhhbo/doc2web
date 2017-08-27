using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public interface ICssClassFactory
    {
        CssClass Build(string styleId);

        CssClass Build(ParagraphProperties pPr);

        CssClass Build(RunProperties rPr);
    }
}
