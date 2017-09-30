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
        private StyleConfig _config;
        private CssRegistrator _instance;

        [TestInitialize]
        public void Initialize()
        {
            _defaultCls = new List<ICssClass>();
            _clsFactory = Substitute.For<ICssClassFactory>();
            _clsFactory.Defaults.Returns(_defaultCls);
            _config = new StyleConfig();
            _instance = new CssRegistrator(_config, _clsFactory);
        }

        [TestMethod]
        public void RenderInto_StylesTest()
        {
            string expectedStyle1Css = "span.test {color: red;font-weight: bold;}";
            string expectedStyle2Css = "p.test {font-weight: light;text-decoration: underline;}";
            RegisterMockCssClass("style1",
                ("span.test", "font-weight", "bold"),
                ("span.test", "color", "red"));
            RegisterMockCssClass("style2",
                ("p.test", "font-weight", "light"),
                ("p.test", "text-decoration", "underline"));

            _instance.RegisterStyle("style1");
            _instance.RegisterStyle("style2");
            var output = TestRender();

            Assert.IsTrue(output.Contains(expectedStyle1Css));
            Assert.IsTrue(output.Contains(expectedStyle2Css));
            Assert.AreEqual(expectedStyle1Css.Length + expectedStyle2Css.Length, output.Length);
        }

        [TestMethod]
        public void RenderInto_NumberingTest()
        {
            string expectedStyleContainer =
                ".numbering-container.numbering-0-1 {width: 13vw;}";
            string expectedStyleNumber =
                ".numbering-0-1 .numbering-number {color: red;}";

            RegisterMockCssClass(0, 1,
                (".numbering-container.numbering-0-1", "width", "13vw"),
                (".numbering-0-1 .numbering-number", "color", "red"));

            _instance.RegisterNumbering(0, 1);
            var output = TestRender();

            Assert.IsTrue(output.Contains(expectedStyleContainer));
            Assert.IsTrue(output.Contains(expectedStyleNumber));
            Assert.AreEqual(expectedStyleContainer.Length + expectedStyleNumber.Length, output.Length);
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

            var rClsName = _instance.RegisterRunProperties(rProp);
            var pClsName = _instance.RegisterParagraphProperties(pProp);
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
            var cls = _clsFactory.BuildFromRunProperties(rProp);

            var rClsName = _instance.RegisterRunProperties(rProp);
            var rClsName2 = _instance.RegisterRunProperties(rProp);
            var output = TestRender();

            Assert.AreEqual(expectedStyle1Css, output);
            Assert.AreEqual(rClsName2.Length, rClsName.Length);
            for (int i = 0; i < rClsName.Length; i++)
                Assert.AreEqual(rClsName[i], rClsName2[i]);
            cls.Received(1).Selector = _config.RunCssClassPrefix + "." + String.Join("." ,rClsName);
        }

        [TestMethod]
        public void RenderInto_ReuseDynamicClsParagraphTest()
        {
            var pProp = new ParagraphProperties();
            string expectedStyle1Css = @"p {font-weight: light;}span.test {text-decoration: underline;}";
            RegisterMockCssClass(pProp,
                ("p", "font-weight", "light"),
                ("span.test", "text-decoration", "underline"));
            var cls = _clsFactory.BuildFromParagraphProperties(pProp);

            var rClsName = _instance.RegisterParagraphProperties(pProp);
            var rClsName2 = _instance.RegisterParagraphProperties(pProp);
            var output = TestRender();

            Assert.AreEqual(expectedStyle1Css, output);
            Assert.AreEqual(rClsName2.Length, rClsName.Length);
            for (int i = 0; i < rClsName.Length; i++)
                Assert.AreEqual(rClsName[i], rClsName2[i]);

            cls.Received(1).Selector = _config.ParagraphCssClassPrefix + "." + String.Join("." ,rClsName);
        }

        [TestMethod]
        public void Register_DynamicEmtpyTest()
        {
            var pPr = new ParagraphProperties();
            var rPr = new RunProperties();
            var cls = Substitute.For<ICssClass>();
            cls.IsEmpty.Returns(true);
            _clsFactory.BuildFromParagraphProperties(Arg.Is(pPr)).Returns(cls);
            _clsFactory.BuildFromRunProperties(Arg.Is(rPr)).Returns(cls);

            Assert.AreEqual(0, _instance.RegisterParagraphProperties(pPr).Length);
            Assert.AreEqual(0, _instance.RegisterRunProperties(rPr).Length);
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
                .BuildFromStyle(Arg.Is(styleId))
                .Returns(data);
        }

        private void RegisterMockCssClass(
            int numberingInstance, 
            int level, 
            params (string, string, string)[] stubCssData)
        {
            var data = MockCssClass(stubCssData);
            _clsFactory
                .BuildFromNumbering(Arg.Is(numberingInstance), Arg.Is(level))
                .Returns(data);
        }

        private void RegisterMockCssClass(RunProperties rProp, params (string, string, string)[] stubCssData)
        {
            var data = MockCssClass(stubCssData);
            _clsFactory
                .BuildFromRunProperties(Arg.Is(rProp))
                .Returns(data);
        }

        private void RegisterMockCssClass(ParagraphProperties pProp, params (string, string, string)[] stubCssData)
        {
            var data = MockCssClass(stubCssData);
            _clsFactory
                .BuildFromParagraphProperties(Arg.Is(pProp))
                .Returns(data);
        }

        private ICssClass MockCssClass(params (string, string, string)[] stubCssData)
        {
            var cls = Substitute.For<ICssClass>();
            cls
                .When(x => x.InsertCss(Arg.Any<CssData>()))
                .Do(x =>
                {
                    var cssData = x.ArgAt<CssData>(0);
                    foreach (var stubData in stubCssData)
                        cssData.AddAttribute(stubData.Item1, stubData.Item2, stubData.Item3);

                });
            return cls;
        }

    }
}
