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
    public class SpacingCssPropertyTests
    {
        private SpacingCssProperty _instance;

        [TestInitialize]
        public void Initialize()
        {
            _instance = new SpacingCssProperty();
        }

        private void SetValues(string before, string after, string lines)
        {
            _instance.Element = new SpacingBetweenLines()
            {
                Before = before,
                After = after,
                Line = lines
            };
        }

        [TestMethod]
        public void BeforeAfterLines_ValuesTest()
        {
            SetValues("100", "200", "300");

            Assert.AreEqual(0.17638888884479165, _instance.Before);
            Assert.AreEqual(0.35277777768958329, _instance.After);
            Assert.AreEqual(0.52916666653437494, _instance.Line);
        }

        [TestMethod]
        public void BeforeAfterLine_NoValuesTest()
        {
            SetValues(null, null, null);

            Assert.IsNull(_instance.Before);
            Assert.IsNull(_instance.After);
            Assert.IsNull(_instance.Line);
        }

        [TestMethod]
        public void BeforeAfter_AutoSpacingTest()
        {
            _instance.Element = new SpacingBetweenLines()
            {
                Before = "0",
                After = "0",
                AfterAutoSpacing = OnOffValue.FromBoolean(true),
                BeforeAutoSpacing = new OnOffValue()
            };

            Assert.IsNull(_instance.After);
            Assert.IsNull(_instance.Before);
        }

        [TestMethod]
        public void GetSpecificHashCode_Test()
        {
            SetValues(null, "100", "10");
            SpacingCssProperty other = CloneCssProperty();

            Assert.AreEqual(other.GetSpecificHashcode(), _instance.GetSpecificHashcode());
        }

        [TestMethod]
        public void HaveSameOutput_TrueTest()
        {
            SetValues(null, "100", "10");
            SpacingCssProperty other = CloneCssProperty();

            Assert.IsTrue(other.HaveSameOutput(_instance));
        }

        [TestMethod]
        public void HaveSameOutput_FalseTest()
        {
            SetValues(null, "100", "10");
            SpacingCssProperty other = CloneCssProperty();
            SetValues("100", null, "10");

            Assert.IsFalse(other.HaveSameOutput(_instance));
        }

        [TestMethod]
        public void InsertCss_Test()
        {
            SetValues(null, null, "100");
            _instance.Selector = ".test";

            var cssData = _instance.AsCss();

            Assert.AreEqual("0.1764cm", cssData[""][".test"]["line-height"]);

        }

        [TestMethod]
        public void Extends_OverrideTest()
        {
            SetValues("100", "200", "300");
            var other = CloneCssProperty();
            SetValues(null, null, null);

            _instance.Extends(other);

            Assert.AreEqual(0.17638888884479165, _instance.Before);
            Assert.AreEqual(0.35277777768958329, _instance.After);
            Assert.AreEqual(0.52916666653437494, _instance.Line);
        }

        [TestMethod]
        public void Extends_DontOverrideTest()
        {
            SetValues("200", "400", "500");
            var other = CloneCssProperty();
            SetValues("100", "200", "300");

            _instance.Extends(other);

            Assert.AreEqual(0.17638888884479165, _instance.Before);
            Assert.AreEqual(0.35277777768958329, _instance.After);
            Assert.AreEqual(0.52916666653437494, _instance.Line);
        }

        private SpacingCssProperty CloneCssProperty()
        {
            return new SpacingCssProperty
            {
                OpenXmlElement = _instance.Element.CloneNode(true)
            };
        }

    }
}
