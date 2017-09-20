using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Style
{
    [TestClass]
    public class NumberingCssClassTests
    {
        private NumberingCssClass _instance;

        [TestInitialize]
        public void Initialize()
        {
            _instance = new NumberingCssClass();
        }

        [TestMethod]
        public void NumberingCssClass_Test()
        {
            Assert.IsNotNull(_instance.ContainerProps);
            Assert.IsNotNull(_instance.NumberProps);
        }

        [TestMethod]
        public void Selector_GetTest()
        {
            _instance.NumberingId = 1;
            _instance.Level = 0;

            Assert.AreEqual(".numbering-1-0", _instance.Selector);
            Assert.AreEqual(1, _instance.NumberingId);
            Assert.AreEqual(0, _instance.Level);
        }

        [TestMethod]
        public void Selector_SetTest()
        {
            Assert.ThrowsException<InvalidOperationException>(
                () => _instance.Selector = "some thing");
        }

        [TestMethod]
        public void InsertCss_Test()
        {
            var expected = new CssData();
            expected.AddAttribute(".numbering-container.numbering-1-3", "width", "3vw");
            expected.AddAttribute(".numbering-1-3 .numbering-number", "font-family", "Arial");
            _instance.NumberingId = 1;
            _instance.Level = 3;
            _instance.ContainerProps.Add(
                new MockProp1 {
                    Out = ( ".numbering-container.numbering-1-3", "width", "3vw") });
            _instance.NumberProps.Add(
                new MockProp2 {
                    Out = ( ".numbering-1-3 .numbering-number", "font-family", "Arial") });

            var result = _instance.AsCss();

            Assert.AreEqual(expected, result);
        }
    }
}
