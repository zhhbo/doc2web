using Doc2web.Core.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Core.Rendering
{
    [TestClass]
    public class OpeningTagTests
    {

        [TestMethod]
        public void RelatedIndex_Test()
        {
            var instance = new OpeningTag();
            var related = new ClosingTag
            {
                Index = 10
            };
            instance.Related = related;

            Assert.AreEqual(10, instance.RelatedIndex);
        }

        [TestMethod]
        public void Render_Test()
        {
            var instance = new OpeningTag();
            var related = new ClosingTag
            {
                Index = 10
            };
            instance.Related = related;

            var renderOuput = instance.Render();

            Assert.AreEqual("<div>", renderOuput);
        }

        [TestMethod]
        public void Render_AttributesTest()
        {
            var instance = new OpeningTag
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
            Assert.AreEqual("<span id=\"0\" class=\"basic-text\" style=\"color: red\">", renderOutput);
        }
    }
}
