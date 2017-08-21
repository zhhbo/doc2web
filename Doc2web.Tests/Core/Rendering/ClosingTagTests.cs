using Doc2web.Core.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Core.Rendering
{
    [TestClass]
    public class ClosingTagTests
    {
        [TestMethod]
        public void ClosingTag_Test()
        {
            var related = new OpeningTag
            {
                Index = 10,
                Name = "span",
                Z = 100
            };
            var instance = new ClosingTag
            {
                Index = 20,
                Related = related
            };

            Assert.AreSame(related, instance.Related);
            Assert.AreEqual(related.Name, instance.Name);
            Assert.AreEqual(20, instance.Index);
            Assert.AreEqual(related.Index, instance.RelatedIndex);
            Assert.AreEqual(related.Z, instance.Z);
        }

        [TestMethod]
        public void Render_Test()
        {
            var related = new OpeningTag();
            var instance = new ClosingTag
            {
                Related = related
            };

            var renderOutput = instance.Render();

            Assert.AreEqual("</div>", renderOutput);
        }
    }
}
