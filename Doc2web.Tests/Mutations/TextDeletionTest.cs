using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Mutations
{
    [TestClass]
    public class TextDeletionTest
    {
        [TestMethod]
        public void TextDelection_CountTest()
        {
            var instance = new TextDeletion { Position = 1, Count = 10 };
            Assert.AreEqual(10, instance.Offset);
        }
    }
}
