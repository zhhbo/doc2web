using Doc2web.Plugins.Style;
using DocumentFormat.OpenXml.Wordprocessing;
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
        private List<ICssClass> _defaultCls;
        private ICssClassFactory _clsFactory;
        private CssRegistrator _instance;

        [TestInitialize]
        public void Initialize()
        {
            _defaultCls = new List<ICssClass>();
            _clsFactory = Substitute.For<ICssClassFactory>();
            _clsFactory.BuildDefaults().Returns(_defaultCls);
            _instance = new CssRegistrator(_clsFactory);
        }

        [TestMethod]
        public void RenderInto_StylesTest()
        {
            string expectedStyle1Css = @"span.test {color: red;font-weight: bold;}";
            string expectedStyle2Css = @"p.test {font-weight: light;text-decoration: underline;}";
            RegisterMockCssClass("style1",
                ("span.test", "font-weight", "bold"),
                ("span.test", "color", "red"));
            RegisterMockCssClass("style2",
                ("p.test", "font-weight", "light"),
                ("p.test", "text-decoration", "underline"));

            _instance.Register("style1");
            _instance.Register("style2");
            var output = TestRender();

            Assert.IsTrue(output.Contains(expectedStyle1Css));
            Assert.IsTrue(output.Contains(expectedStyle2Css));
            Assert.AreEqual(expectedStyle1Css.Length + expectedStyle2Css.Length, output.Length);
        }


        [TestMethod]
        public void RenderInto_NewDynamicClsTest()
        {
            var pProp = new ParagraphProperties();
            var rProp = new RunProperties();
            string expectedStyle1Css = @"span.asdf {color: red;}";
            string expectedStyle2Css = @"p.asdf1 {font-weight: light;}";
            RegisterMockCssClass(rProp, ("span.asdf", "color", "red"));
            RegisterMockCssClass(pProp, ("p.asdf1", "font-weight", "light"));

            var rClsName = _instance.Register(rProp);
            var pClsName = _instance.Register(pProp);
            var output = TestRender();

            Assert.AreNotEqual("", rClsName);
            Assert.AreNotEqual("", pClsName);
            Assert.IsTrue(output.Contains(expectedStyle1Css));
            Assert.IsTrue(output.Contains(expectedStyle2Css));
            Assert.AreEqual(expectedStyle1Css.Length + expectedStyle2Css.Length, output.Length);
        }

        [TestMethod]
        public void RenderInto_ReuseDynamicClsRunTest()
        {
            var rProp = new RunProperties();
            string expectedStyle1Css = @"span {font-weight: bold;}span.test {color: red;}";
            RegisterMockCssClass(rProp,
                ("span", "font-weight", "bold"),
                ("span.test", "color", "red"));
            var cls = _clsFactory.Build(rProp);

            var rClsName = _instance.Register(rProp);
            var rClsName2 = _instance.Register(rProp);
            var output = TestRender();

            Assert.AreEqual(rClsName2, rClsName);
            Assert.AreEqual(expectedStyle1Css, output);
            cls.Received(1).Selector = "span." + rClsName;
        }

        [TestMethod]
        public void RenderInto_ReuseDynamicClsParagraphTest()
        {
            var pProp = new ParagraphProperties();
            string expectedStyle1Css = @"p {font-weight: light;}span.test {text-decoration: underline;}";
            RegisterMockCssClass(pProp,
                ("p", "font-weight", "light"),
                ("span.test", "text-decoration", "underline"));
            var cls = _clsFactory.Build(pProp);

            var rClsName = _instance.Register(pProp);
            var rClsName2 = _instance.Register(pProp);
            var output = TestRender();

            Assert.AreEqual(rClsName2, rClsName);
            Assert.AreEqual(expectedStyle1Css, output);
            cls.Received(1).Selector = "p." + rClsName;
        }

        [TestMethod]
        public void Register_DynamicEmtpyTest()
        {
            var pPr = new ParagraphProperties();
            var rPr = new RunProperties();
            var cls = Substitute.For<ICssClass>();
            cls.IsEmpty.Returns(true);
            _clsFactory.Build(Arg.Is(pPr)).Returns(cls);
            _clsFactory.Build(Arg.Is(rPr)).Returns(cls);

            Assert.AreEqual("", _instance.Register(pPr));
            Assert.AreEqual("", _instance.Register(rPr));
        }

        [TestMethod]
        public void RenderInto_DefaultsTest()
        {
            var expectedCss = @"p {margin-bottom: 22px;}span {font-famility: arial;}";
            _defaultCls.Add(MockCssClass(
                ("p", "margin-bottom", "22px"),
                ("span", "font-famility", "arial")));

            var css = TestRender();

            Assert.AreEqual(expectedCss, css);
        }

        private string TestRender()
        {
            var sb = new StringBuilder();
            _instance.RenderInto(sb);
            return sb.ToString();
        }

        private void RegisterMockCssClass(string styleId, params (string, string, string)[] stubCssData)
        {
            var data = MockCssClass(stubCssData);
            _clsFactory
                .Build(Arg.Is(styleId))
                .Returns(data);
        }

        private void RegisterMockCssClass(RunProperties rProp, params (string, string, string)[] stubCssData)
        {
            var data = MockCssClass(stubCssData);
            _clsFactory
                .Build(Arg.Is(rProp))
                .Returns(data);
        }

        private void RegisterMockCssClass(ParagraphProperties pProp, params (string, string, string)[] stubCssData)
        {
            var data = MockCssClass(stubCssData);
            _clsFactory
                .Build(Arg.Is(pProp))
                .Returns(data);
        }

        private ICssClass MockCssClass(params (string, string, string)[] stubCssData)
        {
            var cssData = new CssData();
            foreach (var stubData in stubCssData)
                cssData.AddAttribute(stubData.Item1, stubData.Item2, stubData.Item3);
            var cls = Substitute.For<ICssClass>();
            cls.InsertCss().Returns(cssData);
            return cls;
        }

    }
}
