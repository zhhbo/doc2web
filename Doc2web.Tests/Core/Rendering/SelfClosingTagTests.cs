using Doc2web.Core.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Core.Rendering
{

    [TestClass]
    public class SeflClosingTagTests
    {
        [TestMethod]
        public void SelfClosingTag_Test()
        {
            var instance = new SelfClosingTag();

            Assert.AreEqual(0, instance.Index);
            Assert.AreEqual(int.MinValue, instance.Z);
            Assert.AreEqual("div", instance.Name);
            Assert.AreEqual(0, instance.Attributes.Count);
            Assert.AreEqual(instance.Index, instance.RelatedIndex);
        }

        [TestMethod]
        public void Render_DefaultTest()
        {
            var instance = new SelfClosingTag();

            string renderOuput = instance.Render();
            Assert.AreEqual(@"<div />", renderOuput);
        }

        [TestMethod]
        public void Render_AttributesTest()
        {
            var instance = new SelfClosingTag
            {
                Name = "span",
                Attributes = new Dictionary<string, string>()
                {
                    { "id", "0" },
                    { "class", "basic-text" },
                    { "style", "color: red" }
                }
            };

            string renderOutput = instance.Render();
            Assert.AreEqual("<span id=\"0\" class=\"basic-text\" style=\"color: red\"/>", renderOutput);
        }
    }
}
