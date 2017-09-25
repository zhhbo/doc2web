using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Style.Properties
{
    [TestClass]
    public class SpecVanishCssPropertyTests
    {
        [TestMethod]
        public void SetOn_Test()
        {
            var expected = new CssData();
            expected.AddAttribute(".test", "display", "none");
            var instance = new SpecVanishCssProperty { Selector = ".test" };

            var cssData = new CssData();
            instance.SetOn(cssData);

            Assert.AreEqual(expected, cssData);
        }
    }
}
