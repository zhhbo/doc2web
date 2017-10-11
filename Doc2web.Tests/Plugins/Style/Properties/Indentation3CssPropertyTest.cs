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
    public class Indentation3CssPropertyTest
    {
        private Indentation3CssProperty _instance;
        private StyleConfig _config;

        [TestInitialize]
        public void Initialize()
        {
            _config = new StyleConfig();
            _config.LeftIdentationCssClassPrefix = ".numbering-container";
            _instance = new Indentation3CssProperty(_config);
            _instance.Selector = ".test";
        }

        private void SetValues(int? left, int? right, int? firstLine, int? hanging)
        {
            var element = new Indentation();
            if (left.HasValue) element.Left = left.Value.ToString();
            if (right.HasValue) element.Right = right.Value.ToString();
            if (firstLine.HasValue) element.FirstLine = firstLine.Value.ToString();
            if (hanging.HasValue) element.Hanging = hanging.Value.ToString();
            _instance.OpenXmlElement = element;
        }

        [TestMethod]
        public void OpenXmlElement_ValuesTest()
        {
            SetValues(100, 200, 300, 400);

            Assert.AreEqual(Utils.TwipsToPageRatio(100), _instance.Left);
            Assert.AreEqual(Utils.TwipsToPageRatio(200), _instance.Right);
            Assert.AreEqual(Utils.TwipsToPageRatio(300), _instance.FirstLine);
            Assert.AreEqual(Utils.TwipsToPageRatio(400), _instance.Hanging);
        }

        [TestMethod]
        public void OpenXmlElement_HangingValuesTest()
        {
            SetValues(100, null, null, 50);

            Assert.AreEqual(ToVW(_instance.Left), _instance.NoNumberingLeftPadding);
            Assert.AreEqual(ToVW(_instance.Hanging * -1), _instance.NoNumberingTextIndent);

            Assert.AreEqual(ToVW(_instance.Left), _instance.NumberingContainerWith);
            Assert.AreEqual(ToVW(_instance.Hanging), _instance.NumberingNumberWidth);
        }

        [TestMethod]
        public void OpenXmlElement_FirstLineValuesTest()
        {
            SetValues(100, null, 50, null);

            Assert.AreEqual(ToVW(_instance.Left), _instance.NoNumberingLeftPadding);
            Assert.AreEqual(ToVW(_instance.FirstLine), _instance.NoNumberingTextIndent);

            Assert.AreEqual(ToVW(_instance.Left + _instance.FirstLine), _instance.NumberingContainerWith);
            Assert.AreEqual(ToVW(_instance.FirstLine), _instance.NumberingNumberWidth);
        }

        [TestMethod]
        public void OpenXmlElement_NoValuesTest()
        {
            SetValues(null, null, null, null);

            Assert.IsFalse(_instance.Left.HasValue);
            Assert.IsFalse(_instance.Right.HasValue);
            Assert.IsFalse(_instance.FirstLine.HasValue);
            Assert.IsFalse(_instance.Hanging.HasValue);
            Assert.IsNull(_instance.NoNumberingTextIndent);
            Assert.AreEqual("0vw", _instance.NumberingContainerWith);
            Assert.AreEqual("unset", _instance.NumberingNumberWidth);
        }

        [TestMethod]
        public void Clone_Test()
        {
            SetValues(100, 200, 300, 400);
            _instance.Element.Left = "0";
            var other = _instance.Clone() as Indentation3CssProperty;

            Assert.AreSame(_instance.Element, other.Element);
            Assert.AreEqual(_instance.Left, other.Left);
            Assert.AreEqual(_instance.Right, other.Right);
            Assert.AreEqual(_instance.FirstLine, other.FirstLine);
            Assert.AreEqual(_instance.Hanging, other.Hanging);
        }

        [TestMethod]
        public void Extends_Test()
        {
            SetValues(1000, 200, null, null);
            var parent = _instance.Clone() as Indentation3CssProperty;
            SetValues(100, null, 300, null);

            _instance.Extends(parent);

            Assert.AreNotEqual(parent.Left, _instance.Left);
            Assert.AreEqual(parent.Right, _instance.Right);
            Assert.AreNotEqual(parent.FirstLine, _instance.FirstLine);
            Assert.AreEqual(parent.Hanging, _instance.Hanging);
        }

        [TestMethod]
        public void GetHashCode_EqualTest()
        {
            SetValues(100, null, 5, null);
            var other = _instance.Clone();
            SetValues(100, 200, 5, null);

            Assert.AreEqual(other.GetHashCode(), _instance.GetHashCode());
        }

        [TestMethod]
        public void GetHashCode_NotEqualTest()
        {
            SetValues(50, null, null, 0);
            var other1 = _instance.Clone();
            SetValues(100, null, null, 0);
            var other2 = _instance.Clone();
            SetValues(100, null, null, 5);

            Assert.AreNotEqual(other1.GetHashCode(), _instance.GetHashCode());
            Assert.AreNotEqual(other2.GetHashCode(), _instance.GetHashCode());
        }


        [TestMethod]
        public void Equals_FalseTest()
        {
            SetValues(50, 10, 1, 0);
            var clone = _instance.Clone();
            SetValues(50, null, 1, 0);

            Assert.AreNotEqual(_instance, clone);
        }

        [TestMethod]
        public void InsertCss_PaddingOnlyTest()
        {
            SetValues(720, null, null, null);
            var expected = new CssData();
            expected.AddAttribute(
                ".test:not(.numbering)",
                "padding-left",
                "5.88vw");
            expected.AddAttribute(
                $".test.numbering .numbering-container",
                "min-width",
                "5.88vw");

            var data = _instance.AsCss();

            Assert.AreEqual(expected, data);
        }

        [TestMethod]
        public void InsertCss_HangingTest()
        {
            SetValues(720, null, null, 310);
            var expected = new CssData();
            expected.AddAttribute(
                ".test:not(.numbering)",
                "padding-left",
                "5.88vw");
            expected.AddAttribute(
                ".test:not(.numbering)",
                "text-indent",
                "-2.53vw");
            expected.AddAttribute(
                $".test.numbering .numbering-container",
                "min-width",
                "5.88vw");
            expected.AddAttribute(
                $".test.numbering .numbering-number",
                "max-width",
                "2.53vw");

            var data = _instance.AsCss();

            Assert.AreEqual(expected, data);
        }

        [TestMethod]
        public void InsertCss_FirstLineTest()
        {
            SetValues(720, null, 310, null);
            var expected = new CssData();
            expected.AddAttribute(
                ".test:not(.numbering)",
                "padding-left",
                "5.88vw");
            expected.AddAttribute(
                ".test:not(.numbering)",
                "text-indent",
                "2.53vw");
            expected.AddAttribute(
                $".test.numbering .numbering-container",
                "min-width",
                "8.41vw");
            expected.AddAttribute(
                $".test.numbering .numbering-number",
                "max-width",
                "2.53vw");

            var data = _instance.AsCss();

            Assert.AreEqual(expected, data);
        }

        [TestMethod]
        public void InsertCss_RightTest()
        {
            SetValues(null, 720, 310, null);
            var expected = new CssData();
            expected.AddAttribute(
                ".test",
                "padding-right",
                "5.88vw");
            expected.AddAttribute(
                ".test:not(.numbering)",
                "text-indent",
                "2.53vw");

            var data = _instance.AsCss();

            Assert.AreEqual(expected, data);
        }

        private string ToVW(double val) => Math.Round(val * 100, 2) + "vw";

        private string ToVW(double? val) => ToVW(val.Value);
    }
}
