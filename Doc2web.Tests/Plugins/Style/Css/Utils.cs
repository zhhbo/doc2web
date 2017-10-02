using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Css;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Style.Css
{
    public static class Utils
    {
        public static void AssertDynamicClass(StyleConfig config, CssClass2 result)
        {
            Assert.IsTrue(result.Name.StartsWith(config.DynamicCssClassPrefix));
        }

        public static void AssertContainsProps(ICssProperty[] props, CssClass2 result)
        {
            Assert.AreEqual(props.Length, result.Props.Count);

            foreach (var prop in props)
                Assert.IsTrue(result.Props.Contains(prop));
        }
    }
}
