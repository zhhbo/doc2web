using Doc2web.Plugins.Style;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using Doc2web.Tests.Plugins.Style.Css;

namespace Doc2web.Tests.Plugins.Style
{
    [TestClass]
    public class DefaultsProviderTests
    {
        private DefaultsProvider _instance;
        private ICssPropertiesFactory _pPropsFac;
        private ICssPropertiesFactory _rPropsFac;
        private Styles _styles;

        [TestInitialize]
        public void Initialize()
        {
            _pPropsFac = Substitute.For<ICssPropertiesFactory>();
            _rPropsFac = Substitute.For<ICssPropertiesFactory>();
            _styles = Samples.DocumentSample1.GenerateStyles();
            _instance = new DefaultsProvider(FacBuilder, _styles);
            _instance.Init();
        }

        private ICssPropertiesFactory FacBuilder(CssPropertySource arg)
        {
            if (arg == CssPropertySource.Paragraph) return _pPropsFac;
            if (arg == CssPropertySource.Run) return _rPropsFac;
            throw new InvalidOperationException("Unexpected css property source");
        }

        [TestMethod]
        public void Paragraph_SomeTest()
        {
            var expected = new CssPropertiesSet { new MockProp1() };
            _pPropsFac
                .Build(Arg.Is(PDefaults))
                .Returns(expected);

            var result = _instance.Paragraph;

            Assert.IsTrue(expected.Equals(result));
        }

        private ParagraphPropertiesBaseStyle PDefaults =>
            _styles.DocDefaults.ParagraphPropertiesDefault.ParagraphPropertiesBaseStyle;

        [TestMethod]
        public void Paragraph_EmptyTest()
        {
            _styles.DocDefaults.ParagraphPropertiesDefault = null;

            var result = _instance.Paragraph;

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Run_SomeTest()
        {
            var expected = new CssPropertiesSet { new MockProp1() };
            _rPropsFac
                .Build(Arg.Is(RDefaults()))
                .Returns(expected);

            var result = _instance.Run;

            Assert.IsTrue(expected.Equals(result));
        }

        private RunPropertiesBaseStyle RDefaults() => 
            _styles.DocDefaults.RunPropertiesDefault.RunPropertiesBaseStyle;

        [TestMethod]
        public void Run_EmptyTest()
        {
            _styles.DocDefaults.RunPropertiesDefault = null;

            var result = _instance.Run;

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void DefaultParagraphStyle_Test()
        {
            var result = _instance.DefaultParagraphStyle;

            Assert.AreEqual("Normal", result);
        }

        [TestMethod]
        public void DefaultRunStyle_Test()
        {
            var result = _instance.DefaultRunStyle;

            Assert.AreEqual("DefaultParagraphFont", result);
        }
    }
}
