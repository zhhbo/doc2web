using Doc2web.Plugins.Numbering.Mapping.Stringifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Numbering.Mapping.Stringifiers
{
    [TestClass]
    public class NoneStringifierTest
    {
        [TestMethod]
        public void Stringify_Test()
        {
            var instance = new NoneStringifier();
            var r = instance.Render(1);
            Assert.AreEqual("", r);
        }
    }
}
