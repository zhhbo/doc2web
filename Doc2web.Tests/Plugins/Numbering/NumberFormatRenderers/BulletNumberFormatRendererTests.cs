using Microsoft.VisualStudio.TestTools.UnitTesting;
using Doc2web.Plugins.Numbering.NumberFormatRenderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Tests.Plugins.Numbering.NumberFormatRenderers
{
    [TestClass]
    public class BulletNumberFormatRendererTests
    {
        [TestMethod]
        public void RenderTest()
        {
            var instance = new BulletNumberFormatRenderer();

            for (int i = 0; i < 5; i++)
                Assert.AreEqual("•", instance.Render(i));
        }
    }
}