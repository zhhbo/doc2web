using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml;
using Autofac;

namespace Whusion.Core
{
    public class GlobalContext : IGlobalContext
    {
        private IEnumerable<OpenXmlElement> _rootElements;
        private StringBuilder _html;
        private StringBuilder _css;
        private StringBuilder _js;

        public GlobalContext(ILifetimeScope container, IEnumerable<OpenXmlElement> rootElements)
        {
            _rootElements = rootElements;
            _html = new StringBuilder();
            _css = new StringBuilder();
            _js = new StringBuilder();
            Container = container;
        }

        public ILifetimeScope Container { get; set; }

        public IEnumerable<OpenXmlElement> RootElements => _rootElements;

        public string Html => _html.ToString();

        public string Css => _css.ToString();

        public string Js => _js.ToString();

        public void AddHtml(string html) => _html.AppendLine(html);

        public void AddCss(string css) => _css.AppendLine(css);

        public void AddJs(string js) => _js.AppendLine(js);
    }
}

