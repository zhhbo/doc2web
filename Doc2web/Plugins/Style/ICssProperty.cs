using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style
{

    public interface ICssProperty : IComparable
    {
        string Selector { get; set; }
        OpenXmlElement OpenXmlElement { get; set; }
        CssData AsCss();
    }
}
