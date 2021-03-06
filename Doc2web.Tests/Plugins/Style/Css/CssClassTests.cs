﻿using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Css;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Style.Css
{
    [TestClass]
    public class CssClassTests
    {
        private CssClass _instance;

        [TestInitialize]
        public void Initialize()
        {
            _instance = new CssClass();
        }

        [TestMethod]
        public void CssClass2_Test()
        {
            Assert.IsNotNull(_instance.Name);
            Assert.IsNotNull(_instance.Props);
        }

        [TestMethod]
        public void InsertCss_Test()
        {
            MockCssProps();
            var expected = new CssData();
            expected.AddAttribute(".some-class", "color", "red");

            var cssData = new CssData();
            _instance.InsertCss(cssData);

            Assert.AreEqual(expected, cssData);
        }

        private void MockCssProps()
        {
            var cssProp1 = Substitute.For<ICssProperty>();
            _instance.Name = "some-class";
            cssProp1
                .When(x => x.InsertCss(Arg.Any<CssPropertiesSet>(), Arg.Any<CssData>()))
                .Do(x =>
                    x.ArgAt<CssData>(1).AddAttribute(".some-class", "color", "red")
                );
            _instance.Props.Add(cssProp1);
        }
    }
}
