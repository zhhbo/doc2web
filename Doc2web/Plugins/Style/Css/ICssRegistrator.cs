using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Css
{
    public interface ICssRegistrator
    {
        CssClass RegisterParagraph(
            ParagraphProperties inlineProps, 
            (int, int)? numbering=null);

        CssClass RegisterRun(
            ParagraphProperties parentProps, 
            OpenXmlElement inlineProps,
            (int, int)? numbering);

        void InsertCss(CssData cssData);
    }
}
