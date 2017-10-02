using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Css;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Style.Css
{
    [TestClass]
    public class BaseNumberingPropsCacheTests
    {
        private Dictionary<Level, CssPropertiesSet> _values;
        private INumberingProvider _nProvider;
        private List<Level> _levels;
        private BaseNumberingPropsCache _instance;

        public class MockNumberingPropsCache : BaseNumberingPropsCache
        {
            private IDictionary<Level, CssPropertiesSet> _values;

            public MockNumberingPropsCache(
                IDictionary<Level, CssPropertiesSet> values,
                INumberingProvider numberingProvider) : base(numberingProvider)
            {
                _values = values;
            }
            public override CssPropertiesSet BuildPropertiesSet(Level arg)
            {
                if (_values.TryGetValue(arg, out CssPropertiesSet result))
                    return result;
                return new CssPropertiesSet();
            }

        }

        [TestInitialize]
        public void Initialize()
        {
            _values = new Dictionary<Level, CssPropertiesSet>();
            _nProvider = Substitute.For<INumberingProvider>();
            _levels = BuildLevels();
            _instance = new MockNumberingPropsCache(_values, _nProvider);
        }

        [TestMethod]
        public void GetContainer_NewTest()
        {
            var prop1 = MockLevel(0, new MockProp1());
            var prop2 = MockLevel(1, new MockProp2());
            MockNumberingProvider(7, 1);

            var result = _instance.Get(7, 1);

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains(prop1));
            Assert.IsTrue(result.Contains(prop2));
            Assert.AreEqual(1, _instance.Cache.Count);
            Assert.AreSame(result, _instance.Cache[(7, 1)]);
        }

        [TestMethod]
        public void GetContainer_CachingTest()
        {
            var prop1 = MockLevel(0, new MockProp3());
            var prop2 = MockLevel(2, new MockProp4());
            MockNumberingProvider(2, 2);
            var result = _instance.Get(2, 2);
            _nProvider.ClearReceivedCalls();

            var result2 = _instance.Get(2, 2);

            Assert.AreSame(result, result2);
            _nProvider.DidNotReceive().Collect(2, 2);
            Assert.AreEqual(1, _instance.Cache.Count);
            Assert.AreSame(result, _instance.Cache[(2, 2)]);
        }

        private ICssProperty MockLevel(int index, ICssProperty prop)
        {
            _values.Add(_levels[index], new CssPropertiesSet { prop });
            return prop;
        }

        private void MockNumberingProvider(int numberingId, int levelIndex)
        {
            _nProvider.Collect(numberingId, levelIndex).Returns(_levels);
        }

        private List<Level> BuildLevels()
        {
            return new List<Level>()
            {
                BuildLevel(true, true),
                BuildLevel(false, true),
                BuildLevel(true, false),
            };
        }

        private Level BuildLevel(bool hasPProps, bool hasRProps) // sorry uncle bob
        {
            var level = new Level();
            if (hasPProps) level.PreviousParagraphProperties = new PreviousParagraphProperties();
            if (hasRProps) level.NumberingSymbolRunProperties = new NumberingSymbolRunProperties();
            return level;
        }
    }
}
