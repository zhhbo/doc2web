using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Style
{
    [TestClass]
    public class ParagraphCssClassTests
    {
        [TestMethod]
        public void ParagraphCssClass_Test()
        {
            var instance = new ParagraphCssClass();

            Assert.IsNull(instance.Selector);
            Assert.IsNull(instance.BasedOn);
            Assert.IsNotNull(instance.RunProps);
            Assert.IsNotNull(instance.ParagraphProps);
        }

        [TestMethod]
        public void AsCss_Test()
        {
            var expectedCssData = new CssData();
            expectedCssData.AddAttribute("p.test-class", "background", "gray");
            expectedCssData.AddAttribute("p.test-class span", "font-weight", "bold");
            var pCssProp = new MockProp1 { Out = ("p.test-class", "background", "gray") };
            var rCssProp = new MockProp2 { Out = ("p.test-class span", "font-weight", "bold") };
            var instance = new ParagraphCssClass { Selector = "p.test-class" };
            instance.ParagraphProps.Add(pCssProp);
            instance.RunProps.Add(rCssProp);

            var cssData = instance.AsCss();

            Assert.AreEqual(expectedCssData, cssData);
            Assert.AreEqual("p.test-class", pCssProp.Selector);
            Assert.AreEqual("p.test-class", rCssProp.Selector);
        }

        [TestMethod]
        public void AssCss_BasedOnCombineTest()
        {
            var expectedCssData = new CssData();
            expectedCssData.AddAttribute("p.test-class", "background", "pale");
            expectedCssData.AddAttribute("p.test-class", "border", "1px solid black");
            expectedCssData.AddAttribute("p.test-class span", "text-transform", "uppercase");
            expectedCssData.AddAttribute("p.test-class span", "font-weight", "bold");

            var defaults = new ParagraphCssClass();
            defaults.ParagraphProps.Add(new MockProp1 { Out = ("p.test-class", "background", "pale") });

            var basedOn = new ParagraphCssClass();
            basedOn.RunProps.Add(new MockProp2 { Out = ("p.test-class span", "text-transform", "uppercase") });
            basedOn.BasedOn = defaults;

            var instance = new ParagraphCssClass();
            instance.ParagraphProps.Add(new MockProp3 { Out = ("p.test-class", "border", "1px solid black") });
            instance.RunProps.Add(new MockProp4 { Out = ("p.test-class span", "font-weight", "bold") });
            instance.BasedOn = basedOn;

            var cssData = instance.AsCss();

            Assert.AreEqual(expectedCssData, cssData);
        }

        [TestMethod]
        public void AssCss_DefaultCombineTest()
        {
            var expectedCssData = new CssData();
            expectedCssData.AddAttribute("p.test-class", "background", "pale");
            expectedCssData.AddAttribute("p.test-class", "border", "1px solid black");
            expectedCssData.AddAttribute("p.test-class span", "text-transform", "uppercase");
            expectedCssData.AddAttribute("p.test-class span", "font-weight", "bold");

            var defaults = new ParagraphCssClass();
            defaults.ParagraphProps.Add(new MockProp1 { Out = ("p.test-class", "background", "pale") });
            defaults.RunProps.Add(new MockProp2 { Out = ("p.test-class span", "text-transform", "uppercase") });

            var instance = new ParagraphCssClass();
            instance.ParagraphProps.Add(new MockProp3 { Out = ("p.test-class", "border", "1px solid black") });
            instance.RunProps.Add(new MockProp4 { Out = ("p.test-class span", "font-weight", "bold") });
            instance.Defaults = defaults;

            var cssData = instance.AsCss();

            Assert.AreEqual(expectedCssData, cssData);
        }

        [TestMethod]
        public void AssCss_BasedOnOverrideTest()
        {
            var expectedCssData = new CssData();
            expectedCssData.AddAttribute("p.test-class span", "font-weight", "bold");
            expectedCssData.AddAttribute("p.test-class", "background", "pale");

            var basedOn = new ParagraphCssClass();
            basedOn.RunProps.Add(new MockProp1 { Out = ("p.test-class", "some", "stuff") });
            basedOn.RunProps.Add(new MockProp2 { Out = ("p.test-class span", "text-transform", "uppercase") });

            var instance = new ParagraphCssClass();
            instance.RunProps.Add(new MockProp1 { Out = ("p.test-class span", "font-weight", "bold") });
            instance.RunProps.Add(new MockProp2 { Out = ("p.test-class", "background", "pale") });

            instance.BasedOn = basedOn;
            var cssData = instance.AsCss();

            Assert.AreEqual(expectedCssData, cssData);
        }

        [TestMethod]
        public void AssCss_DefaultsOverrideTest()
        {
            var expectedCssData = new CssData();
            expectedCssData.AddAttribute("p.test-class span", "font-weight", "bold");
            expectedCssData.AddAttribute("p.test-class", "background", "pale");

            var defaults = new ParagraphCssClass();
            defaults.RunProps.Add(new MockProp1 { Out = ("p.test-class", "some", "stuff") });
            defaults.RunProps.Add(new MockProp2 { Out = ("p.test-class span", "text-transform", "uppercase") });

            var instance = new ParagraphCssClass();
            instance.RunProps.Add(new MockProp1 { Out = ("p.test-class span", "font-weight", "bold") });
            instance.RunProps.Add(new MockProp2 { Out = ("p.test-class", "background", "pale") });

            instance.Defaults = defaults;
            var cssData = instance.AsCss();

            Assert.AreEqual(expectedCssData, cssData);
        }

        [TestMethod]
        public void Equals_TrueTest()
        {
            var prop = Substitute.For<ICssProperty>();
            var cls1 = new ParagraphCssClass();
            var cls2 = new ParagraphCssClass();
            cls1.ParagraphProps.Add(prop);
            cls2.ParagraphProps.Add(prop);

            Assert.AreEqual(cls1, cls2);
        }

        [TestMethod]
        public void Equals_FalseTest()
        {
            var prop = Substitute.For<ICssProperty>();
            var cls1 = new ParagraphCssClass();
            var cls2 = new RunCssClass();
            cls1.ParagraphProps.Add(prop);

            Assert.AreNotEqual(cls1, cls2);
        }

        [TestMethod]
        public void IsEmpty_TrueTest()
        {
            var cls = new ParagraphCssClass();

            Assert.IsTrue(cls.IsEmpty);
        }

        [TestMethod]
        public void IsEmpty_FalseTest()
        {
            var cls1 = new ParagraphCssClass();
            cls1.ParagraphProps.Add(new MockProp1());
            var cls2 = new ParagraphCssClass();
            cls2.RunProps.Add(new MockProp1());

            Assert.IsFalse(cls1.IsEmpty);
            Assert.IsFalse(cls2.IsEmpty);
        }

        [TestMethod]
        public void Hashcode_ColisionTest()
        {
            var cls1a = new ParagraphCssClass();
            var cls1b = new ParagraphCssClass();

            Assert.AreEqual(cls1a.GetHashCode(), cls1b.GetHashCode());
        }
    }
}
