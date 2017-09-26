using Doc2web.Plugins.Numbering.Mapping.Stringifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Doc2web.Tests.Plugins.Numbering.Mapping.Stringifiers
{
    [TestClass]
    public class BulletStringifierTests
    {
        [TestMethod]
        public void RenderTest()
        {
            var instance = new BulletStringifier();

            for (int i = 0; i < 5; i++)
                Assert.AreEqual("•", instance.Render(i));
        }
    }
}