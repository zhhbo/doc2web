using Doc2web.Plugins.Style.Css;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using Doc2web.Plugins.Style;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Linq;

namespace Doc2web.Tests.Plugins.Style.Css
{
    [TestClass]
    public class ParagraphClassFactoryTests
    {
        private IStylePropsCache _stylePropsCache;
        private INumberingPropsCache _numPropsCache;
        private IDefaultsProvider _defaults;
        private StyleConfig _config;
        private ICssPropertiesFactory _propsFac;
        private ParagraphClassFactory _instance;

        [TestInitialize]
        public void Initialize()
        {
            _config = new StyleConfig();
            _propsFac = Substitute.For<ICssPropertiesFactory>();
            _propsFac.Build(null).ReturnsForAnyArgs(x => new CssPropertiesSet());
            _stylePropsCache = Substitute.For<IStylePropsCache>();
            _numPropsCache = Substitute.For<INumberingPropsCache>();
            _defaults = Substitute.For<IDefaultsProvider>();
            _defaults.Paragraph.Returns(new CssPropertiesSet());
            _instance = new ParagraphClassFactory(
                _config,
                _defaults,
                _stylePropsCache,
                _numPropsCache,
                FactoryBuilder);
        }

        private ICssPropertiesFactory FactoryBuilder(CssPropertySource arg)
        {
            if (arg != CssPropertySource.Paragraph)
                throw new ArgumentException("the expected source is paragraph");
            return _propsFac;
        }

        [TestMethod]
        public void Build_EmptyTest()
        {
            var result = _instance.Build(new ParagraphClassParam
            {
                InlineProperties = new ParagraphProperties()
            });

            Assert.AreEqual(0, result.Props.Count);
        }

        [TestMethod]
        public void Build_DynFromPropsTest()
        {
            var pPr = new ParagraphProperties();
            var props = new CssPropertiesSet
            {
                new MockProp1(),
                new MockProp2()
            };
            _propsFac.Build(Arg.Is(pPr)).Returns(props);

            var result = _instance.Build(new ParagraphClassParam
            {
                InlineProperties = pPr
            });

            Utils.AssertDynamicClass(_config, result);
            Assert.IsTrue(props.SetEquals(result.Props));
        }

        [TestMethod]
        public void Build_NumberingPropsTest()
        {
            var propSet = new CssPropertiesSet { new MockProp1(), new MockProp2() };
            _numPropsCache.Get(7, 2).Returns(propSet);

            var result = _instance.Build(new ParagraphClassParam
            {
                InlineProperties = new ParagraphProperties(),
                NumberingId = 7,
                NumberingLevel = 2
            });

            Assert.IsTrue(propSet.SetEquals(result.Props));
            Utils.AssertDynamicClass(_config, result);
        }

        [TestMethod]
        public void Build_FromStyleTest()
        {
            string styleId = "yolo";
            var pPr = new ParagraphProperties
            {
                ParagraphStyleId = new ParagraphStyleId { Val = styleId }
            };
            var props = new CssPropertiesSet
            {
                new MockProp1(),
                new MockProp2()
            };
            _propsFac.Build(Arg.Is(pPr)).Returns(new CssPropertiesSet());
            _stylePropsCache.Get(styleId).Returns(props);

            var result = _instance.Build(new ParagraphClassParam
            {
                StyleId = styleId,
                InlineProperties = pPr
            });

            Assert.AreEqual(styleId, result.Name);
            Assert.IsTrue(props.SetEquals(result.Props));
        }

        [TestMethod]
        public void Build_SetDefaults()
        {
            var pPr = new ParagraphProperties();
            var props = new CssPropertiesSet
            {
                new MockProp1(),
                new MockProp2()
            };
            _propsFac.Build(Arg.Is(pPr)).Returns(props);
            _defaults.Paragraph.Returns(new CssPropertiesSet
            {
                new MockProp3(),
                new MockProp4()
            });
            var expected = new CssPropertiesSet(props.Clone().ToArray());
            expected.AddMany(_defaults.Paragraph);

            var result = _instance.Build(new ParagraphClassParam
            {
                InlineProperties = pPr
            });

            Utils.AssertDynamicClass(_config, result);
            Assert.IsTrue(result.Props.SetEquals(expected));
        }
    }
}
