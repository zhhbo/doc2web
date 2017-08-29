using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Properties;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            var instance = new RunCssClass();
            instance.RunProps.Add(new MockProp1{ Out = ("span.test-class", "font-weight", "bold")});
            instance.RunProps.Add(new MockProp2{ Out = ("span.test-class", "text-transform", "uppercase")});

            var cssData = instance.AsCss();

            Assert.AreEqual(expectedCssData, cssData);
        }

        [TestMethod]
        public void AssCss_BasedOnCombineTest()
        {
            var expectedCssData = new CssData();
            expectedCssData.AddAttribute("span.test-class", "border", "1 px solid");
            expectedCssData.AddAttribute("span.test-class", "background", "pale");
            expectedCssData.AddAttribute("span.test-class", "text-transform", "uppercase");
            expectedCssData.AddAttribute("span.test-class", "font-weight", "bold");

            var basedOn = new RunCssClass();
            basedOn.RunProps.Add(new MockProp1{ Out = ("span.test-class", "border", "1 px solid")});
            basedOn.RunProps.Add(new MockProp2{ Out = ("span.test-class", "background", "pale")});

            var instance = new RunCssClass();
            instance.RunProps.Add(new MockProp3{ Out = ("span.test-class", "font-weight", "bold")});
            instance.RunProps.Add(new MockProp4{ Out = ("span.test-class", "text-transform", "uppercase")});

            instance.BasedOn = basedOn;
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
    }
}
