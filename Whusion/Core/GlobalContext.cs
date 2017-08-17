using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml;
using Autofac;

namespace Whusion.Core
{
    public class GlobalContext : IGlobalContext
    {
        private IContainer _container;
        private IEnumerable<OpenXmlElement> _rootElements;

        public GlobalContext(IContainer container, IEnumerable<OpenXmlElement> rootElements)
        {
            _container = container;
            _rootElements = rootElements;
            Html = new StringBuilder();
            Css = new StringBuilder();
            Js = new StringBuilder();
        }

        public IContainer Container => _container;

        public IEnumerable<OpenXmlElement> RootElements => _rootElements;

        public StringBuilder Html { get; private set; }

        public StringBuilder Css { get; private set; }

        public StringBuilder Js { get; private set; }

        public void AddHtml(string html) => Html.AppendLine(html);

        public void AddCss(string css) => Css.AppendLine(css);

        public void AddJs(string js) => Js.AppendLine(js);
    }
}

