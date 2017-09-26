﻿using Autofac;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web
{
    public interface IGlobalContext
    {
        IEnumerable<OpenXmlElement> RootElements { get; }

        string Html { get; }

        string Css { get; }

        string Js { get; }

        void AddHtml(string html);

        void AddCss(string css);

        void AddJs(string js);

        T Resolve<T>();
    }
}
