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
                Position = 10,
                Name = "span",
                //Z = 100
            };
            var instance = new ClosingTag
            {
                Position = 20,
                Related = related,
            };

            Assert.AreSame(related, instance.Related);
            Assert.AreEqual(related.Name, instance.Name);
            Assert.AreEqual(20, instance.Position);
            Assert.AreEqual(related.Position, instance.RelatedPosition);
            //Assert.AreEqual(related.Z, instance.Z);
        }

        [TestMethod]
        public void Render_Test()
        {
            var related = new OpeningTag();
            var instance = new ClosingTag
            {
                Related = related,
                TextBefore = "some stuff"
            };

            var renderOutput = instance.Render();

            Assert.AreEqual("some stuff</div>", renderOutput);
        }
    }
}
