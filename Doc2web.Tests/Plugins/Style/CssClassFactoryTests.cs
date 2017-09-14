using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using NSubstitute;
using Doc2web.Plugins.Style;
using System.Linq;
using DocumentFormat.OpenXml;

namespace Doc2web.Tests.Plugins.Style
{
    [TestClass]
    public class CssClassFactoryTests
    {
        private Styles _styles;
        private ICssPropertiesFactory _propsFac;
        private StyleConfiguration _config;
        private CssClassFactory _instance;

        [TestInitialize]
        public void Initialize()
        {
            _styles = Samples.StyleBasedOn1.CreateStyles();
            _propsFac = Substitute.For<ICssPropertiesFactory>();
            _config = new StyleConfiguration();
            _instance = new CssClassFactory(_styles, _config, _propsFac);
        }

        [TestMethod]
        public void Build_FromRunStyleIdTest()
        {
            var mockBasedOn = MockRunCssProps("RunStyle1");
            var mockProp = MockRunCssProps("RunStyle2");

            var cls = _instance.Build("RunStyle2");
            Assert.IsNotNull(cls);

            var runCssClass = cls as RunCssClass;
            Assert.IsNotNull(runCssClass);
            Assert.AreEqual(mockProp, runCssClass.RunProps.Single());

            var basedOnCls = runCssClass.BasedOn;
            Assert.IsNotNull(basedOnCls);
            Assert.AreEqual(mockBasedOn, basedOnCls.RunProps.Single());
        }

        [TestMethod]
        public void Build_FromParagraphStyleIdTest()
        {
            var mockBasedOnPara = MockParagraphCssProps("ParagraphStyle1");
            var mockBasedOnRun = MockRunCssProps("ParagraphStyle1");

            var mockPropPara = MockParagraphCssProps("ParagraphStyle2");
            var mockPropRun = MockRunCssProps("ParagraphStyle2");

            var cls = _instance.Build("ParagraphStyle2");
            Assert.IsNotNull(cls);

            var paraCssCls = cls as ParagraphCssClass;
            Assert.IsNotNull(paraCssCls);
            Assert.AreEqual(mockPropRun, paraCssCls.RunProps.Single());
            Assert.AreEqual(mockPropPara, paraCssCls.ParagraphProps.Single());

            var basedOnCls = paraCssCls.BasedOn;
            Assert.IsNotNull(basedOnCls);
            Assert.AreEqual(mockBasedOnRun, basedOnCls.RunProps.Single());
            Assert.AreEqual(mockBasedOnPara, basedOnCls.ParagraphProps.Single());
        }

        [TestMethod]
        public void Build_FromRunPropsTest()
        {
            var mockProps = new ICssProperty[] { Substitute.For<ICssProperty>() };
            var runProps = new RunProperties();
            _propsFac.Build(Arg.Is(runProps)).Returns(mockProps);

            var cls = _instance.Build(runProps);
            Assert.IsNotNull(cls);

            var runCssClass = cls as RunCssClass;
            Assert.IsNotNull(runCssClass);
            Assert.AreEqual(mockProps[0], runCssClass.RunProps.Single());
        }

        [TestMethod]
        public void Build_FromParagraphPropsTest()
        {
            var mockProps = new ICssProperty[] { new MockProp1() };
            var pPr = new ParagraphProperties();
            _propsFac.Build(Arg.Is(pPr)).Returns(mockProps);

            var cls = _instance.Build(pPr);
            Assert.IsNotNull(cls);

            var pCssClass = cls as ParagraphCssClass;
            Assert.IsNotNull(pCssClass);
            Assert.AreEqual(mockProps[0], pCssClass.ParagraphProps.Single());
            Assert.AreEqual(0, pCssClass.RunProps.Count());
        }

        [TestMethod]
        public void BuildDefault_Test()
        {
            var pDocDefaults = _styles.DocDefaults.ParagraphPropertiesDefault;
            var pCssProp = new MockProp1();
            _propsFac
                .Build(Arg.Is(pDocDefaults.ParagraphPropertiesBaseStyle))
                .Returns(new ICssProperty[] { pCssProp });
            var rDocDefaults = _styles.DocDefaults.RunPropertiesDefault;
            var rCssProp = new MockProp2();
            _propsFac
                .Build(Arg.Is(rDocDefaults.RunPropertiesBaseStyle))
                .Returns(new ICssProperty[] { rCssProp });

            var defaults = _instance.BuildDefaults();

            Assert.AreEqual(2, defaults.Count);
            AssertContainsSinglePProps(pCssProp, defaults[0]);
            AssertContainsSingleRProp(rCssProp, defaults[1]);
        }

        private void AssertContainsSingleRProp(MockProp2 rCssProp, ICssClass cls)
        {
            var rClsDefault = cls as RunCssClass;
            Assert.IsNotNull(rClsDefault);
            Assert.AreEqual(_config.RunCssClassPrefix, rClsDefault.Selector);
            Assert.AreEqual(rCssProp, rClsDefault.RunProps.Single());
        }

        private void AssertContainsSinglePProps(MockProp1 pCssProp, ICssClass cls)
        {
            var pClsDefault = cls as ParagraphCssClass;
            Assert.IsNotNull(pClsDefault);
            Assert.AreEqual(_config.ParagraphCssClassPrefix, pClsDefault.Selector);
            Assert.AreEqual(pCssProp, pClsDefault.ParagraphProps.Single());
            Assert.AreEqual(0, pClsDefault.RunProps.Count);
        }

        private ICssProperty MockParagraphCssProps(string styleName) =>
            MockGenCssProps( styleName, s => s.StyleParagraphProperties);

        private ICssProperty MockRunCssProps(string styleName) =>
            MockGenCssProps( styleName, s => s.StyleRunProperties);

        private ICssProperty MockGenCssProps(
            string styleName,
            Func<DocumentFormat.OpenXml.Wordprocessing.Style, OpenXmlElement> map)
        {
            var basedOnProps = new ICssProperty[] { new MockProp1 () };
            var runPropsBasedOn =
                _styles
                .Elements<DocumentFormat.OpenXml.Wordprocessing.Style>()
                .Single(x => x.StyleId?.Value == styleName);
            _propsFac
                .Build(Arg.Is(map(runPropsBasedOn)))
                .Returns(basedOnProps);
            return basedOnProps[0];
        }

    }
}
