﻿using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Css;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WStyle = DocumentFormat.OpenXml.Wordprocessing.Style;

namespace Doc2web.Tests.Plugins.Style.Css
{
    [TestClass]
    public class PStylePPropsCacheTests
    {
        private Func<CssPropertySource, ICssPropertiesFactory> _facBuilder;
        private PStylePPropsCache _instance;
        private ICssPropertiesFactory _propsFac;

        [TestInitialize]
        public void Initialize()
        {
            _propsFac = Substitute.For<ICssPropertiesFactory>();
            _facBuilder = buildPropsFactory;
            _instance = new PStylePPropsCache(_facBuilder, Enumerable.Empty<WStyle>());
        }

        private ICssPropertiesFactory buildPropsFactory(CssPropertySource arg)
        {
            if (arg == CssPropertySource.Paragraph) return _propsFac;
            throw new ArgumentException("Not expected propety source, should be a paragraph");
        }

        [TestMethod]
        public void BuildProps_Test()
        {
            var style = new WStyle
            {
                StyleParagraphProperties = new DocumentFormat.OpenXml.Wordprocessing.StyleParagraphProperties()
            };

            _instance.BuildProps(style);

            _propsFac.Received(1).Build(style.StyleParagraphProperties);
        }
    }
}
