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
        private ICssPropertiesFactory _paraPropFac;
        private ICssPropertiesFactory _runPropFac;
        private ICssPropertiesFactory _numPropFac;
        private StyleConfig _config;
        private INumberingProvider _numCrawler;
        private CssClassFactory _instance;
        private ParagraphPropertiesDefault _pDocDefaults;
        private MockProp1 _pCssPropDefaults;
        private RunPropertiesDefault _rDocDefaults;
        private MockProp2 _rCSsPropDefaults;

        [TestInitialize]
        public void Initialize()
        {
            _styles = Samples.StyleBasedOn1.CreateStyles();
            _paraPropFac = Substitute.For<ICssPropertiesFactory>();
            _runPropFac = Substitute.For<ICssPropertiesFactory>();
            _numPropFac = Substitute.For<ICssPropertiesFactory>();

            _config = new StyleConfig();
            _numCrawler = Substitute.For<INumberingProvider>();
            MockDefaults();
            _instance = new CssClassFactory(_styles, _config, CssPropFac, _numCrawler);
            _instance.Initialize();
        }

        public void MockDefaults ()
        {
            _pDocDefaults = _styles.DocDefaults.ParagraphPropertiesDefault;
            _pCssPropDefaults = new MockProp1();
            _paraPropFac
                .Build(Arg.Is(_pDocDefaults.ParagraphPropertiesBaseStyle))
                .Returns(new ICssProperty[] { _pCssPropDefaults });
            _rDocDefaults = _styles.DocDefaults.RunPropertiesDefault;
             _rCSsPropDefaults = new MockProp2();
            _runPropFac
                .Build(Arg.Is(_rDocDefaults.RunPropertiesBaseStyle))
                .Returns(new ICssProperty[] { _rCSsPropDefaults });
        }

        private ICssPropertiesFactory CssPropFac(CssPropertySource source)
        {
            switch (source)
            {
                case CssPropertySource.Paragraph: return _paraPropFac;
                case CssPropertySource.Run: return _runPropFac;
                case CssPropertySource.Numbering: return _numPropFac;
            }
            return null;
        }


        [TestMethod]
        public void BuildDefault_Test()
        {
            var defaults = _instance.Defaults;

            Assert.AreEqual(2, defaults.Count);
            Assert.IsNotNull(_instance.RunDefaults);
            Assert.IsNotNull(_instance.ParagraphDefault);

            Assert.AreSame(_instance.ParagraphDefault, defaults[0]);
            AssertContainsSinglePProps(_pCssPropDefaults, defaults[0]);
            Assert.AreSame(_instance.RunDefaults, defaults[1]);
            AssertContainsSingleRProp(_rCSsPropDefaults, defaults[1]);
        }

        [TestMethod]
        public void Build_FromRunStyleIdTest()
        {
            var mockBasedOn = MockRunCssProps("RunStyle1");
            var mockProp = MockRunCssProps("RunStyle2");

            var cls = _instance.BuildFromStyle("RunStyle2");
            Assert.IsNotNull(cls);

            var runCssClass = cls as RunCssClass;
            Assert.IsNotNull(runCssClass);
            Assert.AreEqual(mockProp, runCssClass.RunProps.Single());

            var basedOnCls = runCssClass.BasedOn;
            Assert.IsNotNull(basedOnCls);
            Assert.AreEqual(mockBasedOn, basedOnCls.RunProps.Single());

            Assert.AreSame(_instance.RunDefaults, runCssClass.Defaults);
        }

        [TestMethod]
        public void Build_FromParagraphStyleIdTest()
        {
            var mockBasedOnPara = MockParagraphCssProps("ParagraphStyle1");
            var mockBasedOnRun = MockRunCssProps("ParagraphStyle1");

            var mockPropPara = MockParagraphCssProps("ParagraphStyle2");
            var mockPropRun = MockRunCssProps("ParagraphStyle2");

            var cls = _instance.BuildFromStyle("ParagraphStyle2");
            Assert.IsNotNull(cls);

            var paraCssCls = cls as ParagraphCssClass;
            Assert.IsNotNull(paraCssCls);
            Assert.AreEqual(mockPropRun, paraCssCls.RunProps.Single());
            Assert.AreEqual(mockPropPara, paraCssCls.ParagraphProps.Single());

            var basedOnCls = paraCssCls.BasedOn;
            Assert.IsNotNull(basedOnCls);
            Assert.AreEqual(mockBasedOnRun, basedOnCls.RunProps.Single());
            Assert.AreEqual(mockBasedOnPara, basedOnCls.ParagraphProps.Single());

            Assert.AreSame(_instance.ParagraphDefault, paraCssCls.Defaults);
        }

        [TestMethod]
        public void Build_FromNumberingTest()
        {
            var level1 = new Level(
                new PreviousParagraphProperties(),
                new NumberingSymbolRunProperties()
           );
            var level2 = new Level(
                new PreviousParagraphProperties(),
                new NumberingSymbolRunProperties()
            );
            var mockProp1 = new MockProp1();
            var mockProp2 = new MockProp2();
            var mockProp3 = new MockProp3();
            var mockProp4 = new MockProp4();
            _numCrawler
                .Collect(Arg.Is(0), Arg.Is(1))
                .Returns(new List<Level>() { level1, level2 });
            _numPropFac
                .Build(Arg.Is(level1.PreviousParagraphProperties))
                .Returns(new ICssProperty[] { mockProp1 });
            _numPropFac
                .Build(Arg.Is(level2.PreviousParagraphProperties))
                .Returns(new ICssProperty[] { mockProp2 });
            _runPropFac
                .Build(Arg.Is(level1.NumberingSymbolRunProperties))
                .Returns(new ICssProperty[] { mockProp3 });
            _runPropFac
                .Build(Arg.Is(level2.NumberingSymbolRunProperties))
                .Returns(new ICssProperty[] { mockProp4 });

            var result = _instance.BuildFromNumbering(0, 1) as NumberingCssClass;

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.NumberingId);
            Assert.AreEqual(1, result.Level);
            Assert.AreEqual(2, result.ContainerProps.Count);
            Assert.IsTrue(result.ContainerProps.Contains(mockProp1));
            Assert.IsTrue(result.ContainerProps.Contains(mockProp2));
            Assert.AreEqual(2, result.NumberProps.Count);
            Assert.IsTrue(result.NumberProps.Contains(mockProp3));
            Assert.IsTrue(result.NumberProps.Contains(mockProp4));
        }

        [TestMethod]
        public void Build_FromRunPropsTest()
        {
            var mockProps = new ICssProperty[] { Substitute.For<ICssProperty>() };
            var runProps = new RunProperties();
            _runPropFac.Build(Arg.Is(runProps)).Returns(mockProps);

            var cls = _instance.BuildFromRunProperties(runProps);
            Assert.IsNotNull(cls);

            var runCssClass = cls as RunCssClass;
            Assert.IsNotNull(runCssClass);
            Assert.AreEqual(mockProps[0], runCssClass.RunProps.Single());

            Assert.AreSame(_instance.RunDefaults, runCssClass.Defaults);
        }

        [TestMethod]
        public void Build_FromParagraphPropsTest()
        {
            var mockProps = new ICssProperty[] { new MockProp1() };
            var pPr = new ParagraphProperties();
            _paraPropFac.Build(Arg.Is(pPr)).Returns(mockProps);

            var cls = _instance.BuildFromParagraphProperties(pPr);
            Assert.IsNotNull(cls);

            var pCssClass = cls as ParagraphCssClass;
            Assert.IsNotNull(pCssClass);
            Assert.AreEqual(mockProps[0], pCssClass.ParagraphProps.Single());
            Assert.AreEqual(0, pCssClass.RunProps.Count());

            Assert.AreSame(_instance.ParagraphDefault, pCssClass.Defaults);
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
            var r = map(runPropsBasedOn);
            if (r is StyleParagraphProperties)
            {
                _paraPropFac
                    .Build(Arg.Is(r))
                    .Returns(basedOnProps);
            } else
            {
                _runPropFac
                    .Build(Arg.Is(r))
                    .Returns(basedOnProps);
            }
            
            return basedOnProps[0];
        }

    }
}
