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
            var expected = new ICssProperty[] { new MockProp1() };
            _pPropsFac
                .Build(Arg.Is(_styles.DocDefaults.ParagraphPropertiesDefault))
                .Returns(expected);

            var result = _instance.Paragraph;

            Utils.AssertContainsProps(expected, result);
        }

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
            var expected = new ICssProperty[] { new MockProp1() };
            _rPropsFac
                .Build(Arg.Is(_styles.DocDefaults.RunPropertiesDefault))
                .Returns(expected);

            var result = _instance.Run;

            Utils.AssertContainsProps(expected, result);
        }

        [TestMethod]
        public void Run_EmptyTest()
        {
            _styles.DocDefaults.RunPropertiesDefault = null;

            var result = _instance.Run;

            Assert.AreEqual(0, result.Count);
        }
    }
}
