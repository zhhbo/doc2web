using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Properties;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Style.Properties
{
    [TestClass]
    public class BoldBooleanCssPropertyTests
    {
        private BoldCssProperty _instance;
        private BoldCssProperty _instance2;

        [TestInitialize]
        public void Initialize()
        {
            _instance = new BoldCssProperty();
            _instance.Element = new Bold() { Val = new OnOffValue() };
            _instance.Selector = "span.test-cls";

            _instance2 = new BoldCssProperty();
            _instance2.Element = new Bold() { Val = new OnOffValue() };
        }

        [TestMethod]
        public void AsCss_Test()
        {
            var expectedCssData = new CssData();
            expectedCssData.AddAttribute("span.test-cls", "font-weight", "bold");

            var css = _instance.AsCss();

            Assert.AreEqual(expectedCssData, css);
        }

        [TestMethod]
        public void AsCss_OnTest()
        {
            _instance.Element = new Bold { Val = new OnOffValue(true) };
            var expectedCssData = new CssData();
            expectedCssData.AddAttribute("span.test-cls", "font-weight", "bold");

            var css = _instance.AsCss();

            Assert.AreEqual(expectedCssData, css);

        }

        [TestMethod]
        public void AsCss_OffTest()
        {
            _instance.Element = new Bold { Val = new OnOffValue(false) };
            var expectedCssData = new CssData();
            expectedCssData.AddAttribute("span.test-cls", "font-weight", "normal");

            var css = _instance.AsCss();

            Assert.AreEqual(expectedCssData, css);

        }

        [TestMethod]
        public void Equals_TrueTest1()
        {

            Assert.IsTrue(_instance.Equals(_instance2));
        }

        [TestMethod]
        public void Equals_TrueTest2()
        {
            _instance.Element = new Bold { Val = new OnOffValue(false) };
            _instance2.Element = new Bold { Val = new OnOffValue(false) };

            Assert.IsTrue(_instance.Equals(_instance2));

            _instance.Element = new Bold { Val = new OnOffValue(true) };
            _instance2.Element = new Bold { Val = new OnOffValue(true) };

            Assert.IsTrue(_instance.Equals(_instance2));
           
        }

        [TestMethod]
        public void Equals_TrueTest3()
        {
            _instance2.Element = new Bold { Val = new OnOffValue(true) };
            Assert.IsFalse(_instance.Equals(_instance2));
        }

        [TestMethod]
        public void Equals_FalseTest1()
        {
            _instance2.Element = new Bold { Val = new OnOffValue(false) };

            Assert.IsFalse(_instance.Equals(_instance2));
        }

        [TestMethod]
        public void Equals_FalseTest2()
        {
            _instance.Element = new Bold { Val = new OnOffValue(false) };
            _instance2.Element = new Bold { Val = new OnOffValue(true) };

            Assert.IsFalse(_instance.Equals(_instance2));
        }

        [TestMethod]
        public void Extends_TrueOverrideEmtpyTest()
        {
            _instance2.Element = new Bold { Val = new OnOffValue(true) };

            _instance.Extends(_instance2);

            Assert.IsTrue(_instance.ExplicitVal.HasValue);
            Assert.IsTrue(_instance.ExplicitVal.Value);
        }

        [TestMethod]
        public void Extends_FalseOverrideEmtpyTest()
        {
            _instance2.Element = new Bold { Val = new OnOffValue(false) };

            _instance.Extends(_instance2);

            Assert.IsTrue(_instance.ExplicitVal.HasValue);
            Assert.IsFalse(_instance.ExplicitVal.Value);
        }

        [TestMethod]
        public void Extends_DontOverrideTrueTest()
        {
            _instance.Element = new Bold { Val = new OnOffValue(true) };
            _instance2.Element = new Bold { Val = new OnOffValue(false) };

            _instance.Extends(_instance2);

            Assert.IsTrue(_instance.ExplicitVal.HasValue);
            Assert.IsTrue(_instance.ExplicitVal.Value);
        }

        [TestMethod]
        public void Extends_DontOverrideFalseTest()
        {
            _instance.Element = new Bold { Val = new OnOffValue(false) };
            _instance2.Element = new Bold { Val = new OnOffValue(true) };

            _instance.Extends(_instance2);

            Assert.IsTrue(_instance.ExplicitVal.HasValue);
            Assert.IsFalse(_instance.ExplicitVal.Value);
        }
    }
}
