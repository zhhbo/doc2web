using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doc2web.Plugins.Numbering.Mapping;
using Doc2web.Tests.Samples;

namespace Doc2web.Tests.Plugins.Numbering.Mapping
{
    [TestClass]
    public class ConfigCacheTests
    {

        [TestMethod]
        public void Get_CacheLevel1Test()
        {
            var numberingPart = NumberingSample1.GenerateNumbering();
            var abstractNum = numberingPart.Elements<AbstractNum>().Single();
            var numberingInstance = numberingPart.Elements<NumberingInstance>().Single();

            var numberingConfigFac = Substitute.For<IConfigFactory>();
            var mockAbstractNumConfig = new Config();
            numberingConfigFac.CreateFromAbstractNumbering(Arg.Is(abstractNum)).Returns(mockAbstractNumConfig);
            var mockNumberingConfig = new Config();
            numberingConfigFac
              .CreateFromNumbering(Arg.Is(mockAbstractNumConfig), Arg.Is(numberingInstance))
              .Returns(mockNumberingConfig);

            var instance = new ConfigCache(numberingPart, null, numberingConfigFac);

            instance.Get(numberingInstance.NumberID.Value);
            var result = instance.Get(numberingInstance.NumberID.Value);

            Assert.AreSame(mockNumberingConfig, result);

            numberingConfigFac.Received(1).CreateFromAbstractNumbering(Arg.Is(abstractNum));
        }

        [TestMethod]
        public void Get_NumberingStyleLinkTest()
        {
            var numberingPart = NumberingSample2.GenerateNumbering();
            var stylePart = NumberingSample2.GenerateStyles();
            var numberingConfig = new Config();

            var anum1 = numberingPart
              .Elements<AbstractNum>()
              .Single(x => x.AbstractNumberId.Value == 18);

            var numI1 = numberingPart
              .Elements<NumberingInstance>()
              .Single(x => x.NumberID.Value == 8);

            var anum2 = numberingPart
              .Elements<AbstractNum>()
              .Single(x => x.AbstractNumberId.Value == 10);

            var numI2 = numberingPart
              .Elements<NumberingInstance>()
              .Single(x => x.NumberID.Value == 7);

            var numberingConfigFac = Substitute.For<IConfigFactory>();

            numberingConfigFac.CreateFromAbstractNumbering(Arg.Is(anum1))
              .Returns(x => { throw new LinkedStyleNumberingException("ListBullets"); });
            numberingConfigFac.CreateFromAbstractNumbering(Arg.Is(anum2))
              .Returns(numberingConfig);
            numberingConfigFac.CreateFromNumbering(Arg.Is(numberingConfig), Arg.Is(numI2))
              .Returns(numberingConfig);
            numberingConfigFac.CreateFromNumbering(Arg.Is(numberingConfig), Arg.Is(numI1))
              .Returns(numberingConfig);

            var instance = new ConfigCache(numberingPart, stylePart, numberingConfigFac);
            var result = instance.Get(numI1.NumberID.Value);

            Assert.AreEqual(numberingConfig, result);
        }

        [TestMethod]
        public void Get_NumberingCircleException()
        {
            var numberingPart = NumberingCircularSample.GenerateNumbering();
            var stylePart = NumberingCircularSample.GenerateStyles();
            var numConfigFac = Substitute.For<IConfigFactory>();
            numConfigFac
              .CreateFromAbstractNumbering(Arg.Any<AbstractNum>())
              .Returns(_ => throw new LinkedStyleNumberingException("ImportedStyle1"));

            var numberingConfigCache = new ConfigCache(numberingPart, stylePart, numConfigFac);

            Assert.ThrowsException<CircularNumberingException>(() => numberingConfigCache.Get(1));
        }
    }
}