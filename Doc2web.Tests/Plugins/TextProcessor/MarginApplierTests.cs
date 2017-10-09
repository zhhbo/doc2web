using Doc2web.Core;
using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Css;
using Doc2web.Plugins.Style.Properties;
using Doc2web.Plugins.TextProcessor;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Tests.Plugins.TextProcessor
{
    [TestClass]
    public class MarginApplierTests
    {
        private TextProcessorConfig _config = new TextProcessorConfig();
        private MarginApplier _instance;
        private IElementContext[] _elements;

        [TestInitialize]
        public void Initialize()
        {
            _config = new TextProcessorConfig();
        }

        private void MockContext(params (double?, double?)[] spacing)
        {
            _elements = BuildMultipleContext(spacing);
            _instance = new MarginApplier(_config, _elements);
            _instance.Apply();
        }

        private IElementContext[] BuildMultipleContext(params (double?, double?)[] spacing)
        {
            var result = new IElementContext[spacing.Length];
            for(int i = 0; i < spacing.Length; i++)
                result[i] = BuildPContext(spacing[i].Item1, spacing[i].Item2);
            return result;
        }

        private IElementContext BuildPContext(double? before, double? after)
        {
            var elementContext = Substitute.For<IElementContext>();
            var node = new HtmlNode();
            node.AddClasses(_config.ContainerCls);

            string b = (before.HasValue) ? (before * 20).ToString() : null;
            string a = (after.HasValue) ? (after * 20).ToString() : null;

            return BuildPContext(elementContext, node, b, a);
        }

        private IElementContext BuildPContext(IElementContext elementContext, HtmlNode node, string b, string a)
        {
            elementContext.Nodes.Returns(new HtmlNode[] { node, new HtmlNode() });
            elementContext.ViewBag.Returns(new Dictionary<string, object>()
            {
                { _config.PPropsCssClassKey, new CssClass
                {
                    Name = "some-name",
                    Props = new CssPropertiesSet(new SpacingCssProperty
                    {
                        Element = new SpacingBetweenLines()
                        {
                            Before = b,
                            After = a
                        }
                    })
                } }
            });
            return elementContext;
        }

        private double? GetMargin(IElementContext context, string attributeName = "margin-bottom")
        {
            var node = context.Nodes.FirstOrDefault(x => x.Classes.Contains(_config.ContainerCls));
            if (node == null) return null;
            if (node.Style.TryGetValue(attributeName, out string val))
                return Double.Parse(val.Substring(0, val.Length-2));
            return null;
        }

        [TestMethod]
        public void Apply_FirstTopMarginTest()
        {
            MockContext((2, 0));

            Assert.AreEqual(2, GetMargin(_elements[0], "margin-top"));
        }

        [TestMethod]
        public void Apply_LastBottomMarginTest()
        {
            MockContext((0, 2));

            Assert.AreEqual(2, GetMargin(_elements[0]));
        }

        [TestMethod]
        public void Apply_UseGreaterTests()
        {
            MockContext(
                (0, 2),
                (3, 3),
                (2, 0)
            );

            Assert.AreEqual(3, GetMargin(_elements[0]));
            Assert.AreEqual(3, GetMargin(_elements[1]));
        }

        [TestMethod]
        public void Apply_NullTests()
        {
            MockContext(
                (0, null),
                (3, 3),
                (null, 0)
            );

            Assert.AreEqual(3, GetMargin(_elements[0]));
            Assert.AreEqual(3, GetMargin(_elements[1]));
        }
    }
}
