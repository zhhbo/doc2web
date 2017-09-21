using Doc2web.Plugins.Numbering;
using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Properties;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Style.Properties
{
    [TestClass]
    public class NumberingMarginCssPropertyTests
    {
        [TestMethod]
        public void InsertCss_LeftTest()
        {
            var expected = new CssData();
            expected.AddAttribute(".numbering-x-x.numbering-container", "flex-direction", "row-reverse");
            expected.AddAttribute(".numbering-x-x.numbering-container", "width", "17.645vw");
            expected.AddAttribute(".numbering-x-x .numbering-number", "width", "4.632vw");
            expected.AddAttribute("(min-width: 21.59cm)", 
                ".numbering-x-x.numbering-container", "width", "3.81cm");
            expected.AddAttribute("(min-width: 21.59cm)",
                ".numbering-x-x .numbering-number", "width", "1cm");
            var indentation = new Indentation { Left = "2160", Hanging = "567" };
            var instance = new NumberingMarginCssProperty(
                new StyleConfiguration(), 
                new NumberingProcessorPluginConfig()) {
                Element = indentation
            };
            instance.Selector = ".numbering-x-x";

            var result = instance.AsCss();

            Assert.AreEqual(expected, result);
        }
    }
}
