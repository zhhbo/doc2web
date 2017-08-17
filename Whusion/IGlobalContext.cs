using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whusion
{
    public interface IGlobalContext
    {
        IEnumerable<OpenXmlElement> Elements { get; }
        void AddCss(string css);
        void AddJavascript(string js);
    }
}
