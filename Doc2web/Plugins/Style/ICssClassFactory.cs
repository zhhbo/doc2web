using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public interface ICssClassFactory
    {
        List<ICssClass> BuildDefaults();

        ICssClass Build(string styleId);

        ICssClass Build(ParagraphProperties pPr);

        ICssClass Build(RunProperties rPr);
    }
}
