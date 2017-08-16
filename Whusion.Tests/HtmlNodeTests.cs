using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Whusion.Tests
{
    [TestClass]
    public class HtmlNodeTests
    {
        [TestMethod]
        public void HtmlNode_Test()
        {
            var instance = new HtmlNode();

            Assert.AreEqual("div", instance.Tag);
            Assert.AreEqual(int.MinValue, instance.Z);
        }

        [TestMethod]
        public void AddClass_Test()
        {
            string className = "test-cls";
            var instance = new HtmlNode();

            instance.AddClass(className);

            Assert.IsTrue(instance.Classes.Contains(className));
            Assert.AreEqual(className, instance.Attributes["class"]);
        }


        [TestMethod]
        public void SetStyle_AddTest()
        {
            var instance = new HtmlNode();

            instance.SetStyle("color", "red");

            Assert.AreEqual(instance.Attributes["style"], "color: red");
            Assert.AreEqual("red", instance.Style["color"]);
        }

        [TestMethod]
        public void SetStyle_ReplaceTest()
        {
            var instance = new HtmlNode();

            instance.SetStyle("color", "red");
            instance.SetStyle("color", "blue");

            Assert.AreEqual(instance.Attributes["style"], "color: blue");
            Assert.AreEqual(instance.Style["color"], "blue");
        }

        [TestMethod]
        public void SetStyle_MultipleTest()
        {
            var instance = new HtmlNode();

            instance.SetStyle("color", "red");
            instance.SetStyle("background", "blue");

            Assert.AreEqual(
                "color: red; background: blue",
                instance.Attributes["style"]
                );
            Assert.AreEqual(instance.Style["color"], "red");
            Assert.AreEqual(instance.Style["background"], "blue");
        }

        [TestMethod]
        public void SetStyle_InvalidValueFail()
        {
            var instance = new HtmlNode();

            Assert.ThrowsException<ArgumentException>(() =>
                instance.SetStyle("color", "invalid: value"));
            Assert.ThrowsException<ArgumentException>(() =>
                instance.SetStyle("color", "color; red"));
        }

        [TestMethod]
        public void SetStyle_InvalidNameFail()
        {
            var instance = new HtmlNode();

            Assert.ThrowsException<ArgumentException>(() =>
                instance.SetStyle("invalid: name", "color"));
            Assert.ThrowsException<ArgumentException>(() =>
                instance.SetStyle("invalid; name", "color"));
        }

        [TestMethod]
        public void SetAttribute_Test()
        {
            var instance = new HtmlNode();

            instance.SetAttribute("id", "test");

            Assert.AreEqual("test", instance.Attributes["id"]);
        }

        [TestMethod]
        public void AddAttribute_StyleFailTest()
        {
            var instance = new HtmlNode();

            Assert.ThrowsException<ArgumentException>(() =>
                instance.SetAttribute("style", "color: red"));
        }

        [TestMethod]
        public void AddAttribute_ClassFailTest()
        {
            var instance = new HtmlNode();

            Assert.ThrowsException<ArgumentException>(() =>
                instance.SetAttribute("class", "test"));
        }
    }
}
