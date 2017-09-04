using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Style.Properties
{
    [TestClass]
    public class SmallCapsCssPropertyTest
    {
        private SmallCapsCssProperty _instance;

        [TestInitialize]
        public void Initialize()
        {
            _instance = new SmallCapsCssProperty { Selector = "span.test" };
        }

        [TestMethod]
        public void SetOff_Test()
        {
            var expected = new CssData();
            expected.AddAttribute("span.test", "font-variant", "normal");
            var data = new CssData();

            _instance.SetOff(data);

            Assert.AreEqual(expected, data);
        }

        [TestMethod]
        public void SetOn_Test()
        {
            var expected = new CssData();
            expected.AddAttribute("span.test", "font-variant", "small-caps");
            var data = new CssData();

            _instance.SetOn(data);

            Assert.AreEqual(expected, data);
        }
    }
}
