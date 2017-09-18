using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Mutations
{
    [TestClass]
    public class TextInsertionTest
    {
        [TestMethod]
        public void TextInsertion_RenderTest()
        {
            var instance = new TextInsertion { Text = "new content" };
            Assert.AreEqual(instance.Text, instance.Render());
        }
    }
}
