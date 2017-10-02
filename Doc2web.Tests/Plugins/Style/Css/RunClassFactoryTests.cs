using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Css;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Linq;

namespace Doc2web.Tests.Plugins.Style.Css
{
    [TestClass]
    public class RunClassFactoryTests
    {
        private StyleConfig _config;
        private IDefaultsProvider _defaults;
        private IStylePropsCache _pStylePropsCache;
        private IStylePropsCache _rStylePropsCache;
        private RunClassFactory _instance;
        private ICssPropertiesFactory _propsFac;

        [TestInitialize]
        public void Initialize()
        {
            _config = new StyleConfig();
            _defaults = Substitute.For<IDefaultsProvider>();
            _defaults.Run.Returns(new CssPropertiesSet());
            _pStylePropsCache = Substitute.For<IStylePropsCache>();
            _rStylePropsCache = Substitute.For<IStylePropsCache>();
            _propsFac = Substitute.For<ICssPropertiesFactory>();

            _instance = new RunClassFactory(
                _config,
                _defaults,
                _pStylePropsCache,
                _rStylePropsCache,
                FacBuilder);
        }

        private ICssPropertiesFactory FacBuilder(CssPropertySource arg)
        {
            if (arg == CssPropertySource.Run) return _propsFac;
            throw new ArgumentException("This is not the expected source, should be Run");
        }

        [TestMethod]
        public void Build_DyncPropsTest()
        {
            var rPr = new RunProperties();
            var props = new ICssProperty[]
            {
                new MockProp1(),
                new MockProp2()
            };
            _propsFac.Build(Arg.Is(rPr)).Returns(props);

            var result = _instance.Build(new RunClassParam
            {
                InlineProps = rPr
            });

            Utils.AssertDynamicClass(_config, result);
            Utils.AssertContainsProps(props, result);
        }

        [TestMethod]
        public void Build_RunStyleTest()
        {
            string styleId = "run-style";
            var rPr = new RunProperties
            {
                RunStyle = new RunStyle { Val = styleId }
            };
            var props = new ICssProperty[] {
                new MockProp1(),
                new MockProp2()
            };
            var propsSet = new CssPropertiesSet();
            propsSet.AddMany(props);
            _rStylePropsCache.Get(styleId).Returns(propsSet);

            var result = _instance.Build(new RunClassParam {
                RunStyleId = styleId,
                InlineProps = rPr
            });

            Assert.AreEqual(styleId, result.Name);
            Utils.AssertContainsProps(props, result);
        }


        [TestMethod]
        public void Build_ParagraphStyleTest()
        {
            string styleId = "p-style";
            var rPr = new RunProperties();
            var props = new ICssProperty[] {
                new MockProp1(),
                new MockProp2()
            };
            var propsSet = new CssPropertiesSet();
            propsSet.AddMany(props);
            _pStylePropsCache.Get(styleId).Returns(propsSet);

            var result = _instance.Build(new RunClassParam
            {
                ParagraphStyleId = styleId,
                InlineProps = rPr,
            });

            Utils.AssertDynamicClass(_config, result);
            Utils.AssertContainsProps(props, result);
        }

        [TestMethod]
        public void Build_DefaultTest()
        {
            var rPr = new RunProperties();
            var props = new ICssProperty[]
            {
                new MockProp1(),
                new MockProp2()
            };
            _propsFac.Build(Arg.Is(rPr)).Returns(props);
            _defaults.Run.Returns(new CssPropertiesSet
            {
                new MockProp3(),
                new MockProp4()
            });

            var result = _instance.Build(new RunClassParam
            {
                InlineProps = rPr
            });

            Utils.AssertDynamicClass(_config, result);
            Utils.AssertContainsProps(
                props.Concat(_defaults.Run).ToArray(),
                result);
        }
    }
}
