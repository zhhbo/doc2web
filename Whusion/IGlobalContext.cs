using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whusion
{
    public interface IGlobalContext
    {
        IEnumerable<OpenXmlElement> RootElements { get; }

        void AddHtml(string html);

        void AddCss(string css);

        void AddJs(string js);
    }
}
