﻿using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml;
using Autofac;

namespace Doc2web.Core
{
    public class GlobalContext : IGlobalContext
    {
        private ILifetimeScope _container;
        private IEnumerable<OpenXmlElement> _rootElements;
        private StringBuilder _html;
        private StringBuilder _css;
        private StringBuilder _js;

        public GlobalContext(ILifetimeScope container, IEnumerable<OpenXmlElement> rootElements)
        {
            _container = container;
            _rootElements = rootElements;
            _html = new StringBuilder();
            _css = new StringBuilder();
            _js = new StringBuilder();
        }


        public IEnumerable<OpenXmlElement> RootElements => _rootElements;

        public string Html => _html.ToString();

        public string Css => _css.ToString();

        public string Js => _js.ToString();

        public void AddHtml(string html) => _html.AppendLine(html);

        public void AddCss(string css) => _css.AppendLine(css);

        public void AddJs(string js) => _js.AppendLine(js);

        public T Resolve<T>() => _container.Resolve<T>();
    }
}

