using Doc2web.Plugins.Style.Properties;
using DocumentFormat.OpenXml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Doc2web.Plugins.Style;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Tests.Plugins.Style.Properties
{
    [TestClass]
    public class CssPropertyTests
    {
        private MockCssProperty _instance;

        public class MockCssProperty : CssProperty<Paragraph>
        {
            public override int GetHashCode()
            {
                throw new NotImplementedException();
            }

            public override bool Equals(ICssProperty prop)
            {
                throw new NotImplementedException();
            }

            public override void InsertCss(CssData cssData)
            {
                throw new NotImplementedException();
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            _instance = new MockCssProperty
            {
                Element = new Paragraph()
            };
        }

        [TestMethod]
        public void Element_Test()
        {
            Assert.AreSame(_instance.Element, _instance.OpenXmlElement);
        }

        [TestMethod]
        public void Clone_Test()
        {
            var other = _instance.Clone();

            Assert.AreNotSame(other, _instance);
            Assert.AreSame(other.OpenXmlElement, _instance.Element);
        }
    }
}
