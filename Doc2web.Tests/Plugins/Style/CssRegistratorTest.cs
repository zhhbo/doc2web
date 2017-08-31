using Doc2web.Plugins.Style;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Style
{
    [TestClass]
    public class CssRegistratorTest
    {
        private ICssClassFactory _clsFactory;
        private CssRegistrator _instance;

        [TestInitialize]
        public void Initialize()
        {
            _clsFactory = Substitute.For<ICssClassFactory>();
            _instance = new CssRegistrator(_clsFactory);
        }

        [TestMethod]
        public void RenderInto_StylesTest()
        {
            string expectedCss = @"span {font-weight: bold;}span.test {color: red;}p {font-weight: light;}span.test {text-decoration: underline;}";
            RegisterMockCssClass("style1",
                ("span", "font-weight", "bold"),
                ("span.test", "color", "red"));
            RegisterMockCssClass("style2",
                ("p", "font-weight", "light"),
                ("span.test", "text-decoration", "underline"));

            _instance.Register("style1");
            _instance.Register("style2");
            var output = TestRender();

            Assert.AreEqual(expectedCss, output);
        }

        private string TestRender()
        {
            var sb = new StringBuilder();
            _instance.RenderInto(sb);
            return sb.ToString();
        }

        private void RegisterMockCssClass(string styleId, params (string, string, string)[] stubCssData)
        {
            _clsFactory
                .Build(Arg.Is(styleId))
                .Returns(x => MockCssClass(stubCssData));
        }

        private ICssClass MockCssClass(params (string, string, string)[] stubCssData)
        {
            var cssData = new CssData();
            foreach (var stubData in stubCssData)
                cssData.AddAttribute(stubData.Item1, stubData.Item2, stubData.Item3);
            var cls = Substitute.For<ICssClass>();
            cls.AsCss().Returns(cssData);
            return cls;
        }

    }
}
