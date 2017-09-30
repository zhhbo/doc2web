using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Doc2web.Tests
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

            instance.AddClasses(className);

            Assert.IsTrue(instance.Classes.Contains(className));
            Assert.AreEqual(className, instance.Attributes["class"]);
        }

        [TestMethod]
        public void AddClasses_EmptyTest()
        {
            var instance = new HtmlNode();

            instance.AddClasses("", null);

            Assert.AreEqual(0, instance.Classes.Count);
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

        [TestMethod]
        public void Equal_Test()
        {
            var node1 = new HtmlNode
            {
                Start = 0,
                End = 100,
            };
            var node2 = node1.Clone();

            Assert.IsTrue(node1.Equals(node2));
        }

        [TestMethod]
        public void Clone_Test()
        {
            var node1 = new HtmlNode
            {
                Start = 0,
                End = 100,
            };
            node1.AddClasses("ng-redirect");
            node1.SetAttribute("id", "10");
            node1.SetStyle("color", "red");
            var node2 = node1.Clone();

            Assert.AreEqual(node1, node2);
        }

        [TestMethod]
        public void HasIntersection_TrueTest()
        {
            var instance = new HtmlNode { Start = 5, End = 10 };
            var intersections = new HtmlNode[]
            {
                new HtmlNode { Start = 0, End = 6 },
                new HtmlNode { Start = 4, End = 11 },
                new HtmlNode { Start = 9, End = 11 }
            };

            foreach(var node in intersections)
            {
                Assert.IsTrue(instance.HasIntersection(node));
                Assert.IsTrue(node.HasIntersection(instance));
            }
        }

        [TestMethod]
        public void HasIntersection_FalseTest()
        {
            var instance = new HtmlNode { Start = 5, End = 10 };
            var notIntersections = new HtmlNode[]
            {
                new HtmlNode { Start = 0, End = 5 },
                new HtmlNode { Start = 5, End = 10 },
                new HtmlNode { Start = 10, End = 12 }
            };

            foreach(var node in notIntersections)
            {
                Assert.IsFalse(instance.HasIntersection(node));
                Assert.IsFalse(node.HasIntersection(instance));
            }
        }
    }
}
