﻿using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Style
{
    [TestClass]
    public class CssPropertiesSetTests
    {
        private CssPropertiesSet _instance;

        [TestInitialize]
        public void Initialize()
        {
            _instance = new CssPropertiesSet();
        }

        [TestMethod]
        public void CssPropertiesSet_Test()
        {
            Assert.AreEqual(0, _instance.Count);
            Assert.IsFalse(_instance.IsReadOnly);
            Assert.IsNotNull(_instance.GetEnumerator());
            Assert.IsNotNull((_instance as IEnumerable<ICssProperty>).GetEnumerator());
        }

        [TestMethod]
        public void AddContainsCount_Test()
        {
            var prop = Substitute.For<ICssProperty>();
            _instance.Add(prop);
            Assert.IsTrue(_instance.Contains(prop));
        }

        [TestMethod]
        public void AddMany_Test()
        {
            var props = new ICssProperty[]
            {
                new MockProp1(),
                new MockProp2()
            };

            _instance.AddMany(props);

            foreach (var prop in props)
                Assert.IsTrue(_instance.Contains(prop));
        }

        [TestMethod]
        public void Remove_Test()
        {
            var prop = Substitute.For<ICssProperty>();
            _instance.Add(prop);

            var r = _instance.Remove(prop);

            Assert.IsTrue(r);
            Assert.IsFalse(_instance.Contains(prop));
        }

        [TestMethod]
        public void Clear_Test()
        {
            var prop = Substitute.For<ICssProperty>();
            _instance.Add(prop);
            _instance.Clear();
            Assert.AreEqual(0, _instance.Count);
        }

        [TestMethod]
        public void Compare_DiffTypesTest()
        {
            var bold = new BoldCssProperty();
            var caps = new CapsCssProperty();

            Assert.AreNotEqual(0, _instance.Compare(bold, caps));
        }

        [TestMethod]
        public void AsCssData_Test()
        {
            var prop1CssData = new CssData();
            prop1CssData.AddAttribute(".test", "color", "red");
            prop1CssData.AddAttribute(".test", "", "red");
            var prop1 = Substitute.For<ICssProperty>();
            prop1.AsCss().Returns(prop1CssData);

            _instance.Add(prop1);
            _instance.Selector = ".test";
            var r = _instance.AsCss();

            Assert.AreEqual(prop1CssData, r);
            prop1.Received(1).Selector = ".test";
        }

        [TestMethod]
        public void SetEquals_TrueTest()
        {
            var prop = Substitute.For<ICssProperty>();
            var other = new CssPropertiesSet
            {
                prop
            };
            _instance.Add(prop);

            Assert.IsTrue(_instance.SetEquals(other));
        }

        [TestMethod]
        public void SetEquals_FalseTest()
        {
            var prop = Substitute.For<ICssProperty>();
            var other = new CssPropertiesSet { prop };

            Assert.IsFalse(_instance.SetEquals(other));
        }

        [TestMethod]
        public void GetHashCode_Colision()
        {
            var a = new CssPropertiesSet();
            var b = new CssPropertiesSet();
            var prop1 = new MockProp1();
            var prop2 = new MockProp2();
            a.Add(prop1);
            a.Add(prop2);
            b.Add(prop1);
            b.Add(prop2);

            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }
    }
}