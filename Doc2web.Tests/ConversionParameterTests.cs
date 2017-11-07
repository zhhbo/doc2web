using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests
{
    [TestClass]
    public class ConversionParameterTests
    {
        [TestMethod]
        public void ConversionParameter_Test()
        {
            var p = new ConversionParameter();

            Assert.IsNotNull(p.AdditionalPlugins);
        }
    }
}
