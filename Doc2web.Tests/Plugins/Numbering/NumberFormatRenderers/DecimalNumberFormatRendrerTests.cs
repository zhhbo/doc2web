using Doc2web.Plugins.Numbering.NumberFormatRenderers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Tests.Plugins.Numbering.NumberFormatRenderers
{
    [TestClass()]
    public class DecimalNumberFormatRendererTests
    {
        [TestMethod()]
        public void RenderTest()
        {
            var instance = new DecimalNumberFormatRendrer();
            for (int i = 0; i < 100; i++)
                Assert.AreEqual(instance.Render(i), i.ToString());
        }
    }
}