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
        private IPropsCache _propsCache;
        private IDefaultsProvider _defaults;
        private StyleConfig _config;
        private ICssPropertiesFactory _propsFac;
        private ParagraphClassFactory _instance;

        [TestInitialize]
        public void Initialize()
        {
            _config = new StyleConfig();
            _propsFac = Substitute.For<ICssPropertiesFactory>();
            _propsCache = Substitute.For<IPropsCache>();
            _defaults = Substitute.For<IDefaultsProvider>();
            _defaults.Paragraph.Returns(new CssPropertiesSet());
            _instance = new ParagraphClassFactory(
                _config,
                _defaults,
                _propsCache,
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
            var result = _instance.Build(new ParagraphProperties());

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Build_DynFromPropsTest()
        {
            var pPr = new ParagraphProperties();
            var props = new ICssProperty[]
            {
                new MockProp1(),
                new MockProp2()
            };
            _propsFac.Build(Arg.Is(pPr)).Returns(props);

            var result = _instance.Build(pPr);

            Utils.AssertDynamicClass(_config, result);
            Utils.AssertContainsProps(props, result);
        }


        [TestMethod]
        public void Build_FromStyleTest()
        {
            string styleId = "yolo";
            var pPr = new ParagraphProperties
            {
                ParagraphStyleId = new ParagraphStyleId { Val = styleId }
            };
            var props = new ICssProperty[]
            {
                new MockProp1(),
                new MockProp2()
            };
            var propsSet = new CssPropertiesSet();
            propsSet.AddMany(props);
            _propsFac.Build(Arg.Is(pPr)).Returns(new ICssProperty[0]);
            _propsCache.Get(styleId).Returns(propsSet);

            var result = _instance.Build(pPr);

            Assert.AreEqual(styleId, result.Name);
            Utils.AssertContainsProps(props, result);
        }

        [TestMethod]
        public void Build_SetDefaults()
        {
            var pPr = new ParagraphProperties();
            var props = new ICssProperty[]
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

            var result = _instance.Build(pPr);

            Utils.AssertDynamicClass(_config, result);
            Utils.AssertContainsProps(
                props.Concat(_defaults.Paragraph).ToArray(),
                result);
        }
    }
}
