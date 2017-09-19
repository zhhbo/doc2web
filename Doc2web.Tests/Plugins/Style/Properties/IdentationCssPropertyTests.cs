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
    public class IdentationCssPropertyTests
    {
        private StyleConfiguration _config;
        private IdentationCssProperty _instance;

        [TestInitialize]
        public void Initialize()
        {
            _config = new StyleConfiguration()
            {
                ParagraphCssClassPrefix = "div.container",
                RunCssClassPrefix = "span",
                LeftIdentationCssClassPrefix = "> .leftspacer",
                RightIdentationCssClassPrefix = "> .rightspacer"
            };
            _instance = new IdentationCssProperty(_config);
            _instance.Selector = "div.container.test-class";
            _instance.Element = new Indentation();
        }

        [TestMethod]
        public void InsertCss_LeftTest ()
        {
            var expected = new CssData();
            expected.AddAttribute(
                "div.container.test-class > .leftspacer",
                "max-width",
                "1.27cm");
            expected.AddAttribute(
                "div.container.test-class > .leftspacer",
                "width",
                "5.882%");
            expected.AddAttribute(
                "div.container.test-class > .leftspacer",
                "min-width",
                "fit-content");
            _instance.Element.Left = new StringValue("720");

            var result = _instance.AsCss();

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void InsertCss_RightTest ()
        {
            var expected = new CssData();
            expected.AddAttribute(
                "div.container.test-class > .rightspacer",
                "max-width",
                "1.27cm");
            expected.AddAttribute(
                "div.container.test-class > .rightspacer",
                "width",
                "5.882%");
            expected.AddAttribute(
                "div.container.test-class > .rightspacer",
                "min-width",
                "fit-content");
            _instance.Element.Right = new StringValue("720");

            var result = _instance.AsCss();

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void InsertCss_HangingTest()
        {
            var expected = new CssData();
            expected.AddAttribute(
                "(max-width: 21.59cm)",
                "div.container.test-class",
                "text-indent",
                "-2.941%");
            expected.AddAttribute(
                "(min-width: 21.59cm)",
                "div.container.test-class",
                "text-indent",
                "-0.635cm");
            _instance.Element.Hanging = new StringValue("360");

            var result = _instance.AsCss();

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void InsertCss_FirstLineTest()
        {
            var expected = new CssData();
            expected.AddAttribute(
                "(max-width: 21.59cm)",
                "div.container.test-class",
                "text-indent",
                "2.941%");
            expected.AddAttribute(
                "(min-width: 21.59cm)",
                "div.container.test-class",
                "text-indent",
                "0.635cm");
            _instance.Element.FirstLine = new StringValue("360");

            var result = _instance.AsCss();

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetSpeficicHashcode_Test()
        {
            _instance.Element.Left = new StringValue("720");
            Assert.AreEqual(720, _instance.GetSpecificHashcode());
        }

        [TestMethod]
        public void GetSpeficicHashcode_NoValTest()
        {
            Assert.AreEqual(0, _instance.GetSpecificHashcode());
        }

        [TestMethod]
        public void HaveSameOutput_WithValTrueTest()
        {
            _instance.Element.Left = new StringValue("720");
            _instance.Element.Right = new StringValue("360");
            var other = _instance.Element.CloneNode(true) as Indentation;

            Assert.IsTrue(_instance.HaveSameOuput(other));
        }

        [TestMethod]
        public void HaveSameOutput_NoValTrueTest()
        {
            var other = new Indentation();

            Assert.IsTrue(_instance.HaveSameOuput(other));
        }

        [TestMethod]
        public void HaveSameOutput_WithValFalseTest()
        {
            _instance.Element.Left = new StringValue("720");
            var other = _instance.Element.CloneNode(true) as Indentation;
            other.Right = new StringValue("720");

            Assert.IsFalse(_instance.HaveSameOuput(other));
        }

        [TestMethod]
        public void HaveSameOutput_NoValFalseTest()
        {
            _instance.Element.Left = new StringValue("720");
            var other = new Indentation();

            Assert.IsFalse(_instance.HaveSameOuput(other));
        }
    }
}