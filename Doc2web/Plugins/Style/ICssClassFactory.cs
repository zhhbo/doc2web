using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public interface ICssClassFactory
    {
        List<ICssClass> BuildDefaults();

        ICssClass BuildFromStyle(string styleId);

        ICssClass BuildFromNumbering(int numberingId, int level);

        ICssClass BuildFromParagraphProperties(OpenXmlElement pPr);

        ICssClass BuildFromRunProperties(OpenXmlElement rPr);
    }
}
