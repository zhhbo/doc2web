using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Doc2web.Tests
{
    [TestClass]
    public class StringConversionParameterTests
    {
        [TestMethod]
        public void StringConversionParameter_Test()
        {
            var p = new StringConversionParameter();

            Assert.IsInstanceOfType(p.Stream, typeof(MemoryStream));
            Assert.IsFalse(p.AutoFlush);
            //Assert.AreEqual(0, p.AutoFlushBlockCount);
            Assert.IsNotNull(p.AdditionalPlugins);
        }

        [TestMethod]
        public void PreventSetters_Test()
        {
            var p = new StringConversionParameter();

            Assert.ThrowsException<InvalidOperationException>(() => p.Stream = new MemoryStream());
            Assert.ThrowsException<InvalidOperationException>(() => p.AutoFlush = true);
            //Assert.ThrowsException<InvalidOperationException>(() => p.AutoFlushBlockCount = 10);
        }

        [TestMethod]
        public void GetResult_Test()
        {
            var p = new StringConversionParameter();
            var writter = new StreamWriter(p.Stream);

            writter.Write("test");
            writter.Flush();
            var r = p.GetResult();

            Assert.AreEqual("test", r);
        }
    }
}
