using Doc2web.Plugins.Numbering.Stringifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Tests.Plugins.Numbering.Stringifiers
{
    [TestClass]
    public class DecimalZerolStringifierTests
    {
        [TestMethod]
        public void RenderBiggerThan10Test()
        {
            var instance = new DecimalZeroStringifier();
            for (int i = 10; i < 100; i++)
                Assert.AreEqual(instance.Render(i), i.ToString());
        }

        [TestMethod]
        public void RenderSmallerThan10Test()
        {
            var instance = new DecimalZeroStringifier();
            for (int i = 0; i < 10; i++)
                Assert.AreEqual(instance.Render(i), "0" + i.ToString());
        }
    }
}