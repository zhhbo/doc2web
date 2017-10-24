using Doc2web.Plugins.Style;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Style
{
    [TestClass]
    public class StyleConfigurationTests
    {
        [TestMethod]
        public void StyleConfiguration_Test()
        {
            var a = new StyleConfig();

            Assert.AreEqual("div.container", a.ContainerCssClassPrefix);
            Assert.AreEqual("span", a.RunCssClassPrefix);
            Assert.AreEqual(".leftspacer", a.LeftIdentationCssClassSuffix);
        }

        [TestMethod]
        public void Clone_Test()
        {
            var a = new StyleConfig
            {
                ContainerCssClassPrefix = "a",
                RunCssClassPrefix = "b",
                LeftIdentationCssClassSuffix = "c",
            };

            var b = a.Clone();

            Assert.AreEqual(a.ContainerCssClassPrefix, b.ContainerCssClassPrefix);
            Assert.AreEqual(a.RunCssClassPrefix, b.RunCssClassPrefix);
            Assert.AreEqual(a.LeftIdentationCssClassSuffix, b.LeftIdentationCssClassSuffix);
        }
    }
}
