using Doc2web.Plugins.Style.Properties;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Style.Properties
{
    [TestClass]
    public class HaflPointPropertyTest
    {
        private HaflPointCssProperty<HpsMeasureType> _instance;

        [TestInitialize]
        public void Initialize()
        {
            _instance = new HaflPointCssProperty<HpsMeasureType>();
        }

        [TestMethod]
        public void DefaultVal_Test()
        {
            _instance.Element = new FontSize();

            Assert.AreEqual(_instance.Size, -1);
            Assert.AreEqual(_instance.GetHashCode(), -100);
        }

        [TestMethod]
        public void SizeHashcode_Test()
        {
            SetSize(10);

            Assert.AreEqual(_instance.Size, 5);
            Assert.AreEqual(_instance.GetHashCode(), 500);
        }

        [TestMethod]
        public void Equals_TrueTest()
        {
            SetSize(10);
            var instance2 = _instance.Clone();

            Assert.AreEqual(_instance, instance2);
        }

        [TestMethod]
        public void Equals_FalseTest()
        {
            SetSize(10);
            var instance2 = _instance.Clone();
            SetSize(20);

            Assert.AreNotEqual(_instance, instance2);
        }

        private void SetSize(int val)
        {
            _instance.OpenXmlElement = new FontSizeComplexScript()
            {
                Val = val.ToString()
            };
        }

    }
}
