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
        private INumberingPropsCache _numPropsCache;
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
            _numPropsCache = Substitute.For<INumberingPropsCache>();
            _rStylePropsCache = Substitute.For<IStylePropsCache>();
            _propsFac = Substitute.For<ICssPropertiesFactory>();
            _propsFac.Build(null).ReturnsForAnyArgs(x => new CssPropertiesSet());

            _instance = new RunClassFactory(
                _config,
                new ClsNameGenerator(_config),
                _defaults,
                _pStylePropsCache,
                _numPropsCache,
                _rStylePropsCache,
                FacBuilder);
        }

        private ICssPropertiesFactory FacBuilder(CssPropertySource arg)
        {
            if (arg == CssPropertySource.Run) return _propsFac;
            throw new ArgumentException("This is not the expected source, should be Run");
        }

        [TestMethod]
        public void Build_EmptyTest()
        {
            var result = _instance.Build(new RunClassParam
            {
                InlineProperties = new RunProperties()
            });

            Assert.AreEqual(0, result.Props.Count);
        }

        [TestMethod]
        public void Build_DyncPropsTest()
        {
            var rPr = new RunProperties();
            var props = new CssPropertiesSet
            {
                new MockProp1(),
                new MockProp2()
            };
            _propsFac.Build(Arg.Is(rPr)).Returns(props);

            var result = _instance.Build(new RunClassParam
            {
                InlineProperties = rPr
            });

            Utils.AssertDynamicClass(_config, result);
            Assert.IsTrue(props.SetEquals(result.Props));
        }

        [TestMethod]
        public void Build_RunStyleTest()
        {
            string styleId = "run-style";
            var rPr = new RunProperties
            {
                RunStyle = new RunStyle { Val = styleId }
            };
            var props = new CssPropertiesSet {
                new MockProp1(),
                new MockProp2()
            };
            _rStylePropsCache.Get(styleId).Returns(props);

            var result = _instance.Build(new RunClassParam {
                RunStyleId = styleId,
                InlineProperties = rPr
            });

            Assert.AreEqual(styleId, result.Name);
            Assert.IsTrue(props.SetEquals(result.Props));
        }

        [TestMethod]
        public void Build_NumberingStyleTest()
        {
            var props = new CssPropertiesSet
            {
                new MockProp1(),
                new MockProp2()
            };
            _numPropsCache.Get(7, 2).Returns(props);

            var result = _instance.Build(new RunClassParam
            {
                NumberingId = 7,
                NumberingLevel = 2
            });

            Assert.IsTrue(props.SetEquals(result.Props));
            Utils.AssertDynamicClass(_config, result);
        }


        [TestMethod]
        public void Build_ParagraphStyleTest()
        {
            string styleId = "p-style";
            var rPr = new RunProperties();
            var props = new CssPropertiesSet {
                new MockProp1(),
                new MockProp2()
            };
            _pStylePropsCache.Get(styleId).Returns(props);

            var result = _instance.Build(new RunClassParam
            {
                ParagraphStyleId = styleId,
                InlineProperties = rPr,
            });

            Utils.AssertDynamicClass(_config, result);
            Assert.IsTrue(props.SetEquals(props));
        }

        [TestMethod]
        public void Build_DefaultTest()
        {
            var rPr = new RunProperties();
            var props = new CssPropertiesSet
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
            var expected = props.Clone();
            expected.AddMany(_defaults.Run);

            var result = _instance.Build(new RunClassParam
            {
                InlineProperties = rPr
            });

            Utils.AssertDynamicClass(_config, result);
            Assert.IsTrue(result.Props.SetEquals(expected));
        }
    }
}
