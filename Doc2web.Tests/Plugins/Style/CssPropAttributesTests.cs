using Doc2web.Plugins.Style;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Style
{
    [TestClass]
    public class CssPropAttributesTests
    {
        [TestMethod]
        public void Paragraph_Test()
        {
            var attr = new ParagraphCssPropertyAttribute(typeof(Indentation));

            Assert.AreEqual(CssPropertySource.Paragraph, attr.Source);
            Assert.AreEqual(typeof(Indentation), attr.TargetedType);
        }

        [TestMethod]
        public void Run_Test()
        {
            var attr = new RunCssPropertyAttribute(typeof(Bold));

            Assert.AreEqual(CssPropertySource.Run, attr.Source);
            Assert.AreEqual(typeof(Bold), attr.TargetedType);
        }
    }
}
