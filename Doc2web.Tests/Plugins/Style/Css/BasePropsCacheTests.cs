using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Css;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WStyle = DocumentFormat.OpenXml.Wordprocessing.Style;

namespace Doc2web.Tests.Plugins.Style.Css
{
    [TestClass]
    public class BasePropsCacheTests
    {
        private ICssPropertiesFactory _propsFac;
        private WStyle[] _styles;
        private BasePropsCache _instance;
        private ICssProperty[] _propsA2;
        private ICssProperty[] _propsA3;
        private ICssProperty[] _propsA1;

        public class MockPropsCache : BasePropsCache
        {
            private ICssPropertiesFactory _propsFac;

            public MockPropsCache(
                ICssPropertiesFactory propsFac,
                IEnumerable<WStyle> styles
                ) : base(styles)
            {
                _propsFac = propsFac;
            }

            protected override ICssProperty[] BuildProps(WStyle style) =>
                _propsFac.Build(style);
        }

        [TestInitialize]
        public void Initialize()
        {
            _propsFac = Substitute.For<ICssPropertiesFactory>();
            _styles = new WStyle[]
            {
                MockStyle("a1"),
                MockStyle("a2", "a1"),
                MockStyle("a3", "a1")
            };
            _instance = new MockPropsCache(_propsFac, _styles);
        }

        private WStyle MockStyle(string id, string basedOn = null) =>
            new WStyle
            {
                StyleId = id,
                BasedOn = (basedOn != null) ? new BasedOn { Val = basedOn } : null
            };

        [TestMethod]
        public void ParagraphPropsCache_Test()
        {
            Assert.AreEqual(0, _instance.Cache.Count);
        }

        [TestMethod]
        public void Get_CreateTest()
        {
            string styleId = _styles[0].StyleId;
            var props = MockFactory(
                styleId,
                new MockProp1(),
                new MockProp2());

            var result = _instance.Get(styleId);

            Assert.AreSame(result, _instance.Cache[styleId]);
            AssertContainsProps(result, props);
            Assert.AreEqual(1, _instance.Cache.Count);
            Assert.AreSame(result, _instance.Cache[styleId]);
        }

        [TestMethod]
        public void Get_CachedTest()
        {
            var props = MockFactory(
                "a1",
                new MockProp1()
                );

            _instance.Get("a1");
            _instance.Get("a1");

            _propsFac.Received(1).Build(_styles[0]);
        }

        [TestMethod]
        public void Get_BasedOnCreateTest()
        {
            MockPropsA3_A2_A1();
            var props = _propsA1.Concat(_propsA2).ToArray();

            var result = _instance.Get("a2");

            AssertContainsProps(result, props);
            Assert.AreEqual(2, _instance.Cache.Count);
            Assert.AreSame(result, _instance.Cache["a2"]);
            Assert.IsTrue(_instance.Cache.ContainsKey("a1"));
        }

        [TestMethod]
        public void Get_BasedOnCachedTest()
        {
            MockPropsA3_A2_A1();
            var props = _propsA1.Concat(_propsA3).ToArray();

            _instance.Get("a2");
            var result = _instance.Get("a3");

            AssertContainsProps(result, props);
            Assert.AreEqual(3, _instance.Cache.Count);
            Assert.AreSame(result, _instance.Cache["a3"]);
            Assert.IsTrue(_instance.Cache.ContainsKey("a1"));
        }

        private void MockPropsA3_A2_A1()
        {
            _propsA2 = MockFactory(
                "a2",
                new MockProp1(),
                new MockProp2());
            _propsA3 = MockFactory("a3", _propsA2);
            _propsA1 = MockFactory(
                "a1",
                new MockProp3(),
                new MockProp4()
                );
        }

        [TestMethod]
        public void Get_NotExistingTest()
        {

            var result = _instance.Get("not-there");

            Assert.IsNull(result);
        }

        private ICssProperty[] MockFactory(
            string styleId, 
            params ICssProperty[] props)
        {
            var style = _styles.First(x => x.StyleId == styleId);
            _propsFac
                .Build(style)
                .Returns(props);
            return props;
        }

        private void AssertContainsProps(
            CssPropertiesSet result,
            params ICssProperty[] props)
        {
            Assert.AreEqual(props.Length, result.Count);

            foreach (var p in props)
                Assert.IsTrue(result.Contains(p));
        }
    }
}
