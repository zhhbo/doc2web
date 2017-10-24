using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml;
using Autofac;
using System.Linq;

namespace Doc2web.Core
{
    public class GlobalContext : IGlobalContext
    {
        private ILifetimeScope _container;
        private RootElementContext[] _rootElements;
        private StringBuilder _html;
        private StringBuilder _css;
        private StringBuilder _js;

        public GlobalContext(ILifetimeScope container, IEnumerable<OpenXmlElement> rootElements)
        {
            _container = container;
            _rootElements = rootElements.Select(x => new RootElementContext(this, x)).ToArray();
            _html = new StringBuilder();
            _css = new StringBuilder();
            _js = new StringBuilder();
        }


        public IEnumerable<RootElementContext> RootElements => _rootElements;

        public string Html => _html.ToString();

        public string Css => _css.ToString();

        public string Js => _js.ToString();

        public void AddHtml(string html) => _html.AppendLine(html);

        public void AddCss(string css) => _css.AppendLine(css);

        public void AddJs(string js) => _js.AppendLine(js);

        public T Resolve<T>() => _container.Resolve<T>();

        public bool TryResolve<T>(out T service) => _container.TryResolve(out service);
    }
}

