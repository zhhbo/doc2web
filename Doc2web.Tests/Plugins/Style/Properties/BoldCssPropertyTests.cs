﻿using Doc2web.Plugins.Style;
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
    public class BoldCssPropertyTests
    {
        private BoldCssProperty _instance;
        private BoldCssProperty _instance2;

        [TestInitialize]
        public void Initialize()
        {
            _instance = new BoldCssProperty();
            _instance.Element = new Bold();
            _instance.Selector = "span.test-cls";

            _instance2 = new BoldCssProperty();
            _instance2.Element = new Bold();
            _instance2.Element.Val = new OnOffValue();
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
            _instance.Element.Val = new OnOffValue(true);
            var expectedCssData = new CssData();
            expectedCssData.AddAttribute("span.test-cls", "font-weight", "bold");

            var css = _instance.AsCss();

            Assert.AreEqual(expectedCssData, css);

        }

        [TestMethod]
        public void AsCss_OffTest()
        {
            _instance.Element.Val = new OnOffValue(false);
            var expectedCssData = new CssData();
            expectedCssData.AddAttribute("span.test-cls", "font-weight", "normal");

            var css = _instance.AsCss();

            Assert.AreEqual(expectedCssData, css);

        }

        [TestMethod]
        public void CompareTo_TrueTest1()
        {

            Assert.AreEqual(0, _instance.CompareTo(_instance2));
        }

        [TestMethod]
        public void CompareTo_TrueTest2()
        {
            _instance.Element.Val = new OnOffValue(false);
            _instance2.Element.Val = new OnOffValue(false);

            Assert.AreEqual(0, _instance.CompareTo(_instance2));

            _instance.Element.Val = new OnOffValue(true);
            _instance2.Element.Val = new OnOffValue(true);

            Assert.AreEqual(0, _instance.CompareTo(_instance2));
           
        }

        [TestMethod]
        public void CompareTo_TrueTest3()
        {
            _instance2.Element.Val = new OnOffValue(true);
            Assert.AreNotEqual(0, _instance.CompareTo(_instance2));
        }

        [TestMethod]
        public void CompareTo_FalseTest1()
        {
            _instance2.Element.Val = new OnOffValue(false);

            Assert.AreNotEqual(0, _instance.CompareTo(_instance2));
        }

        [TestMethod]
        public void CompareTo_FalseTest2()
        {
            _instance.Element.Val = new OnOffValue(false);
            _instance2.Element.Val = new OnOffValue(true);

            Assert.AreNotEqual(0, _instance.CompareTo(_instance2));
        }
    }
}