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
        private NumberingMarginCssProperty _instance;

        [TestInitialize]
        public void Initialize()
        {
            _instance = new NumberingMarginCssProperty(
                new StyleConfiguration(),
                new NumberingProcessorPluginConfig())
            {
                Element = new Indentation()
            };

            _instance.Selector = ".numbering-x-x";
        }

        [TestMethod]
        public void InsertCss_LeftTest()
        {
            var expected = new CssData();
            expected.AddAttribute(".numbering-x-x.numbering-container-max", "min-width", "17.645vw");
            expected.AddAttribute(".numbering-x-x .numbering-container-min", "width", "auto");
            expected.AddAttribute(".numbering-x-x .numbering-container-min", "display", "flex");
            expected.AddAttribute(".numbering-x-x .numbering-container-min", "flex-direction", "row-reverse");
            expected.AddAttribute(".numbering-x-x .numbering-number-max", "min-width", "4.632vw");
            expected.AddAttribute(".numbering-x-x .numbering-number-min", "width", "auto");

            expected.AddAttribute("(min-width: 21.59cm)", ".numbering-x-x.numbering-container-max", "width", "3.81cm");
            expected.AddAttribute("(min-width: 21.59cm)", ".numbering-x-x .numbering-number-max", "width", "1cm");
            _instance.Element.Left = "2160";
            _instance.Element.Hanging = "567";

            var result = _instance.AsCss();

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void InsertCss_EmptyLeftTest()
        {
            _instance.Element.Left = "0";

            var result = _instance.AsCss();

            Assert.AreEqual(new CssData(), result);
        }
    }
}
