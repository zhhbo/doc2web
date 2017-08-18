using Autofac;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whusion
{
    public interface IGlobalContext
    {
        IEnumerable<OpenXmlElement> RootElements { get; }

        ILifetimeScope Container { get; set; }

        string Html { get; }

        string Css { get; }

        string Js { get; }

        void AddHtml(string html);

        void AddCss(string css);

        void AddJs(string js);
    }
}
