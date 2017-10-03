using Doc2web.Plugins.Style.Css;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Doc2web.Plugins.Style;
using DocumentFormat.OpenXml.Wordprocessing;
using NSubstitute;
using System.Linq;

namespace Doc2web.Tests.Plugins.Style.Css
{
    [TestClass]
    public class NumberNumberingPropsCacheTests
    {
        private ICssPropertiesFactory _propsFac;
        private NumberNumberingPropsCache _instance;

        [TestInitialize]
        public void Initialize()
        {
            _propsFac = Substitute.For<ICssPropertiesFactory>();
            _instance = new NumberNumberingPropsCache(FactoryBuilder, null);
        }

        private ICssPropertiesFactory FactoryBuilder(CssPropertySource arg)
        {
            if (arg == CssPropertySource.Run) return _propsFac;
            throw new InvalidOperationException("Unexpected source of css properties");
        }

        [TestMethod]
        public void BuildPropsSet_Test()
        {
            var props = new CssPropertiesSet { new MockProp1() };
            var level = new Level {
                NumberingSymbolRunProperties = new NumberingSymbolRunProperties()
            };
            _propsFac
                .Build(Arg.Is(level.NumberingSymbolRunProperties))
                .Returns(props);

            var result = _instance.BuildPropertiesSet(level);

            Assert.AreSame(props.Single(), result.Single());
        }

        [TestMethod]
        public void BuildPropsSet_NullTest()
        {
            var result = _instance.BuildPropertiesSet(new Level());

            Assert.AreEqual(0, result.Count);
        }

    }
}
