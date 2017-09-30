using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Properties;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Style
{
    [TestClass]
    public class RunCssClassTests
    {

        [TestMethod]
        public void RunCssClass_Test()
        {
            var instance = new RunCssClass();

            Assert.IsNull(instance.BasedOn);
            Assert.IsNotNull(instance.RunProps);
        }

        [TestMethod]
        public void AsCss_Test()
        {
            var expectedCssData = new CssData();
            expectedCssData.AddAttribute("span.test-class", "text-transform", "uppercase");
            expectedCssData.AddAttribute("span.test-class", "font-weight", "bold");

            var instance = new RunCssClass { Selector = "span.test-class" };
            instance.RunProps.Add(new MockProp1{ Out = ("span.test-class", "font-weight", "bold")});
            instance.RunProps.Add(new MockProp2{ Out = ("span.test-class", "text-transform", "uppercase")});

            var cssData = instance.AsCss();

            Assert.AreEqual(expectedCssData, cssData);
            foreach(var prop in instance.RunProps)
                Assert.AreEqual("span.test-class", prop.Selector);
        }

        [TestMethod]
        public void AssCss_BasedOnCombineTest()
        {
            var expectedCssData = new CssData();
            expectedCssData.AddAttribute("span.test-class", "border", "1 px solid");
            expectedCssData.AddAttribute("span.test-class", "background", "pale");
            expectedCssData.AddAttribute("span.test-class", "text-transform", "uppercase");
            expectedCssData.AddAttribute("span.test-class", "font-weight", "bold");

            var defaults = new RunCssClass();
            defaults.RunProps.Add(new MockProp2{ Out = ("span.test-class", "background", "pale")});

            var basedOn = new RunCssClass();
            basedOn.RunProps.Add(new MockProp1{ Out = ("span.test-class", "border", "1 px solid")});
            basedOn.BasedOn = defaults;

            var instance = new RunCssClass();
            instance.RunProps.Add(new MockProp3{ Out = ("span.test-class", "font-weight", "bold")});
            instance.RunProps.Add(new MockProp4{ Out = ("span.test-class", "text-transform", "uppercase")});
            instance.BasedOn = basedOn;
            var cssData = instance.AsCss();

            Assert.AreEqual(expectedCssData, cssData);
        }

        [TestMethod]
        public void AssCss_DefaultsCombineTest()
        {
            var expectedCssData = new CssData();
            expectedCssData.AddAttribute("span.test-class", "background", "pale");
            expectedCssData.AddAttribute("span.test-class", "text-transform", "uppercase");
            expectedCssData.AddAttribute("span.test-class", "border", "1 px solid");
            expectedCssData.AddAttribute("span.test-class", "font-weight", "bold");

            var defaults = new RunCssClass();
            defaults.RunProps.Add(new MockProp1{ Out = ("span.test-class", "background", "pale")});
            defaults.RunProps.Add(new MockProp2{ Out = ("span.test-class", "text-transform", "uppercase")});

            var instance = new RunCssClass();
            instance.RunProps.Add(new MockProp3{ Out = ("span.test-class", "border", "1 px solid")});
            instance.RunProps.Add(new MockProp4{ Out = ("span.test-class", "font-weight", "bold")});

            instance.Defaults = defaults;
            var cssData = instance.AsCss();

            Assert.AreEqual(expectedCssData, cssData);
        }

        [TestMethod]
        public void AssCss_BasedOnOverrideTest()
        {
            var expectedCssData = new CssData();
            expectedCssData.AddAttribute("span.test-class", "border", "1 px solid");
            expectedCssData.AddAttribute("span.test-class", "font-weight", "bold");

            var basedOn = new RunCssClass();
            basedOn.RunProps.Add(new MockProp1{ Out = ("span.test-class", "background", "pale")});
            basedOn.RunProps.Add(new MockProp2{ Out = ("span.test-class", "text-transform", "uppercase")});

            var instance = new RunCssClass();
            instance.RunProps.Add(new MockProp1{ Out = ("span.test-class", "border", "1 px solid")});
            instance.RunProps.Add(new MockProp2{ Out = ("span.test-class", "font-weight", "bold")});

            instance.BasedOn = basedOn;
            var cssData = instance.AsCss();

            Assert.AreEqual(expectedCssData, cssData);
        }

        [TestMethod]
        public void AssCss_DefaultsOverrideTest()
        {
            var expectedCssData = new CssData();
            expectedCssData.AddAttribute("span.test-class", "border", "1 px solid");
            expectedCssData.AddAttribute("span.test-class", "font-weight", "bold");

            var defaults = new RunCssClass();
            defaults.RunProps.Add(new MockProp1{ Out = ("span.test-class", "background", "pale")});
            defaults.RunProps.Add(new MockProp2{ Out = ("span.test-class", "text-transform", "uppercase")});

            var instance = new RunCssClass();
            instance.RunProps.Add(new MockProp1{ Out = ("span.test-class", "border", "1 px solid")});
            instance.RunProps.Add(new MockProp2{ Out = ("span.test-class", "font-weight", "bold")});

            instance.Defaults = defaults;
            var cssData = instance.AsCss();

            Assert.AreEqual(expectedCssData, cssData);
        }

        [TestMethod]
        public void Equals_TrueTest()
        {
            var prop = Substitute.For<ICssProperty>();
            var cls1 = new RunCssClass();
            var cls2 = new RunCssClass();
            cls1.RunProps.Add(prop);
            cls2.RunProps.Add(prop);

            Assert.AreEqual(cls1, cls2);
        }

        [TestMethod]
        public void Equals_FalseTest()
        {
            var prop = Substitute.For<ICssProperty>();
            var cls1 = new RunCssClass();
            var cls2 = new RunCssClass();
            cls1.RunProps.Add(prop);

            Assert.AreNotEqual(cls1, cls2);
        }

        [TestMethod]
        public void IsEmpty_TrueTest()
        {
            var cls = new RunCssClass();

            Assert.IsTrue(cls.IsEmpty);
        }

        [TestMethod]
        public void IsEmpty_FalseTest()
        {
            var cls1 = new RunCssClass();
            cls1.RunProps.Add(new MockProp1());

            Assert.IsFalse(cls1.IsEmpty);
        }
    }
}
