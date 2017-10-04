using Doc2web.Plugins.Style;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Style
{
    [TestClass]
    public class ClassNameGeneratorTest
    {
        private StyleConfig _config;
        private ClsNameGenerator _instance;

        [TestInitialize]
        public void Initialize()
        {
            _config = new StyleConfig();
            _config.DynamicCssClassPrefix = "d";
            _instance = new ClsNameGenerator(_config);
        }

        [TestMethod]
        public void Gen_Test()
        {
            int count = 1000;
            int expectedLength = _config.DynamicCssClassPrefix.Length + 7;
            var set = new HashSet<string>(count);

            for(int i = 0; i < count; i++)
            {
                var uid = _instance.GenId();
                Assert.IsFalse(set.Contains(uid));
                Assert.IsTrue(uid.StartsWith(_config.DynamicCssClassPrefix));
                Assert.AreEqual(expectedLength, uid.Length);
            }
        }

    }
}
