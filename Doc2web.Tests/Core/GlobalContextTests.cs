using Autofac;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Doc2web.Core;

namespace Doc2web.Tests.Core
{
    [TestClass]
    public class GlobalContextTests
    {
        private IContainer _container;
        private Paragraph[] _rootElements;
        private GlobalContext _instance;

        [TestInitialize]
        public void Initialize()
        {
            _container = Substitute.For<IContainer>();
            _rootElements = Enumerable.Range(0, 10).Select(x => new Paragraph()).ToArray();
            _instance = new GlobalContext(_container, _rootElements);
        }

        [TestMethod]
        public void GlobalContext_Test()
        {
            Assert.AreSame(_container, _instance.Container);
            Assert.AreSame(_rootElements, _instance.RootElements);
            Assert.AreEqual("", _instance.Html.ToString());
            Assert.AreEqual("", _instance.Css.ToString());
            Assert.AreEqual("", _instance.Js.ToString());
        }

        [TestMethod]
        public void AddHtml_Test()
        {
            string html = @"<div></div>";

            _instance.AddHtml(html);

            Assert.AreEqual(html + "\r\n", _instance.Html.ToString());
        }

        [TestMethod]
        public void AddCss_Test()
        {
            string css = "p { color: red; }";

            _instance.AddCss(css);

            Assert.AreEqual(css + "\r\n", _instance.Css.ToString());
        }

        [TestMethod]
        public void AddJs_Test()
        {
            string js = "window.alert('test')";

            _instance.AddJs(js);

            Assert.AreEqual(js + "\r\n", _instance.Js.ToString());
        }
    }
}
