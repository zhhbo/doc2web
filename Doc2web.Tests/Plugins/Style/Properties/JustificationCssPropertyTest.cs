using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Properties;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Doc2web.Tests.Plugins.Style.Properties
{
    [TestClass]
    public class JustificationCssPropertyTest
    {
        private StyleConfig _config;
        private JustificationCssProperty _instance;

        [TestInitialize]
        public void Initialize()
        {
            _config = new StyleConfig();
            _instance = new JustificationCssProperty(_config);
            _instance.Selector = "p.test";
            _instance.Element = new Justification();
        }

        public JustificationValues JustificationValue
        {
            get => _instance.Element.Val;
            set => _instance.Element.Val = value;
        }

        [TestMethod]
        public void AsCss_EmptyTest()
        {
            Assert.AreEqual(0, _instance.AsCss().Selectors.Count());
        }

        [TestMethod]
        public void AsCss_LeftTest()
        {
            JustificationValue = JustificationValues.Left;
            AssertIsLeftAlign();
        }

        [TestMethod]
        public void AsCss_StartTest()
        {
            JustificationValue = JustificationValues.Start;
            AssertIsLeftAlign();
        }

        [TestMethod]
        public void AsCss_RightTest()
        {
            JustificationValue = JustificationValues.Right;
            AssertIsRightAlign();
        }

        [TestMethod]
        public void AsCss_EndTest()
        {
            JustificationValue = JustificationValues.End;
            AssertIsRightAlign();
        }

        [TestMethod]
        public void AsCss_CenterTest()
        {
            JustificationValue = JustificationValues.Center;
            AssertIsCenterAlign();
        }

        [TestMethod]
        public void AsCss_DistributeTest()
        {
            JustificationValue = JustificationValues.Distribute;
            AssertIsJustifyAlign();
        }

        [TestMethod]
        public void AsCss_BothTest()
        {
            JustificationValue = JustificationValues.Both;
            AssertIsJustifyAlign();
        }

        [TestMethod]
        public void GetHashCode_Test()
        {
            var expected = new(JustificationValues, short)[]
            {
                (JustificationValues.Start, 0),
                (JustificationValues.Left, 0),
                (JustificationValues.End, 1),
                (JustificationValues.Right, 1),
                (JustificationValues.Center, 2),
                (JustificationValues.Distribute, 3),
                (JustificationValues.Both, 3),
            };

            foreach (var (v, e) in expected)
                AssertIsRightSpecificHashcode(v, e);
        }

        private void AssertIsRightSpecificHashcode(JustificationValues v, short expected)
        {
            JustificationValue = v;
            Assert.AreEqual(expected, _instance.GetHashCode());
        }

        private void AssertIsLeftAlign()
        {
            AssertCssEquals(
                ("text-align", "left")
            );
        }

        private void AssertIsRightAlign()
        {
            AssertCssEquals(
                ("text-align", "right")
            );
        }

        private void AssertIsCenterAlign()
        {
            AssertCssEquals(
                (_instance.Selector, "text-align", "center"),
                (_instance.Selector, "justify-content", "center"),
                (_instance.Selector + _config.ParagraphCssClassPrefix, "flex", "0")
            );
        }

        private void AssertIsJustifyAlign()
        {
            AssertCssEquals(
                ("text-align", "justify")
            );
        }
        
        private void AssertCssEquals(params (string, string)[] props)
        {
            var props2 =
                props
                .Select(x => (_instance.Selector, x.Item1, x.Item2))
                .ToArray();
            AssertCssEquals(props2);
        }

        private void AssertCssEquals(params (string, string, string)[] props)
        {
            var expected = new CssData();
            foreach (var (a, b, c) in props)
                expected.AddAttribute(a, b, c);

            var css = _instance.AsCss();
            Assert.AreEqual(expected, css);
        }
    }
}
