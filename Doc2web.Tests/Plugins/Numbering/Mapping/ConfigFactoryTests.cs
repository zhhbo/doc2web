using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doc2web.Tests.Plugins.Numbering.Fixtures;
using Doc2web.Plugins.Numbering.Mapping;

namespace Doc2web.Tests.Plugins.Numbering.Mapping
{
    [TestClass]
    public class ConfigFactoryTests
    {
        [TestMethod]
        public void CreateFromAbstractNumbering_Test()
        {
            var abstractNum = NumberingSample1.GenerateAbstractNum();
            var instance = new ConfigFactory();

            var results = instance.CreateFromAbstractNumbering(abstractNum);

            Assert.AreEqual(abstractNum.AbstractNumberId.Value, results.AbstractNumberingId);
            Assert.IsFalse(results.NumberingId.HasValue);

            var levels = abstractNum.Descendants<Level>().ToArray();
            Assert.AreEqual(levels.Length, results.LevelCount);

            for (var i = 0; i < levels.Length; i++)
            {
                var levelNode = levels[i];
                Assert.IsTrue(results[i].IsFromAbstract);
                Assert.AreEqual(levelNode.LevelIndex.Value, results[i].LevelIndex);
                Assert.AreEqual(levelNode.ParagraphStyleIdInLevel?.Val.Value, results[i].ParagraphStyleId);
                var expectedStart = 1;
                if (levelNode.StartNumberingValue != null && levelNode.StartNumberingValue.Val.HasValue)
                {
                    expectedStart = levelNode.StartNumberingValue.Val.Value;
                }
                Assert.AreEqual(expectedStart, results[i].StartValue);
                Assert.AreEqual(levelNode.NumberingFormat.Val.Value, results[i].NumberingFormat);
                Assert.AreEqual(levelNode.LevelText.Val.Value, results[i].Text);
                Assert.AreSame(levelNode, results[i].LevelNode);
            }
        }

        [TestMethod]
        public void CreateFromNumberingInstance_Test()
        {
            var (instance, numberingInstance, abstractNumConfig) = CreateNumberingInstanceContext();

            var results = instance.CreateFromNumbering(abstractNumConfig, numberingInstance);

            Assert.AreEqual(abstractNumConfig.AbstractNumberingId, results.AbstractNumberingId);
            Assert.AreEqual(numberingInstance.NumberID.Value, results.NumberingId);

            var levelOverrides = numberingInstance.Descendants<LevelOverride>();
            var expectedCount =
                abstractNumConfig.Select(x => x.LevelIndex)
                .Concat(levelOverrides.Select(x => x.LevelIndex.Value))
                .Distinct()
                .Count();
            Assert.AreEqual(expectedCount, results.LevelCount);
        }

        [TestMethod]
        public void CreateFromNumberingInstance_AbstractWithoutOverrideTest()
        {
            var (instance, numberingInstance, abstractNumConfig) = CreateNumberingInstanceContext();
            var @abstract = abstractNumConfig.Single(x => x.LevelIndex == 1);

            var results = instance.CreateFromNumbering(abstractNumConfig, numberingInstance);
            var result = results.Single(x => x.LevelIndex == 1);

            Assert.AreEqual(@abstract.LevelIndex, result.LevelIndex);
            Assert.AreEqual(@abstract.IsFromAbstract, result.IsFromAbstract);
            Assert.AreEqual(@abstract.NumberId, result.NumberId);
            Assert.AreEqual(@abstract.NumberingFormat, result.NumberingFormat);
            Assert.AreEqual(@abstract.ParagraphStyleId, result.ParagraphStyleId);
            Assert.AreEqual(@abstract.StartValue, result.StartValue);
            Assert.AreEqual(@abstract.Text, result.Text);

        }

        [TestMethod]
        public void CreateFromNumbering_OverrideOnlyTest()
        {
            var (instance, numberingInstance, abstractNumConfig) = CreateNumberingInstanceContext();
            var levelOverrides = numberingInstance.Elements<LevelOverride>();
            var levelOverrideWithoutAbstract = levelOverrides.Single(x => x.LevelIndex.Value == 2);

            var results = instance.CreateFromNumbering(abstractNumConfig, numberingInstance);
            var levelOverride = results.Single(x => x.LevelIndex == 2);

            Assert.AreEqual(levelOverrideWithoutAbstract.LevelIndex.Value, levelOverride.LevelIndex);
            Assert.IsFalse(levelOverride.IsFromAbstract);
            Assert.AreEqual(numberingInstance.NumberID.Value, levelOverride.NumberId);
            Assert.AreEqual(levelOverrideWithoutAbstract.Level.NumberingFormat.Val.Value, levelOverride.NumberingFormat);
            Assert.AreEqual(levelOverrideWithoutAbstract.Level.ParagraphStyleIdInLevel.Val.Value, levelOverride.ParagraphStyleId);
            Assert.AreEqual(levelOverrideWithoutAbstract.StartOverrideNumberingValue.Val.Value, levelOverride.StartValue);
            Assert.AreEqual(levelOverrideWithoutAbstract.Level.LevelText.Val.Value, levelOverride.Text);
        }

        [TestMethod]
        public void CreateFromNumbering_MixedTest1()
        {
            var (instance, numberingInstance, abstractNumConfig) = CreateNumberingInstanceContext();
            var levelOverrides = numberingInstance.Elements<LevelOverride>();
            var levelOverride = levelOverrides.Single(x => x.LevelIndex.Value == 3);

            var results = instance.CreateFromNumbering(abstractNumConfig, numberingInstance);
            var result = results.Single(x => x.LevelIndex == 3);
            var @abstract = abstractNumConfig.Single(x => x.LevelIndex == 3);

            Assert.AreEqual(levelOverride.LevelIndex.Value, result.LevelIndex);
            Assert.IsFalse(result.IsFromAbstract);

            Assert.AreEqual(@abstract.LevelIndex, result.LevelIndex);
            Assert.AreEqual(numberingInstance.NumberID.Value, result.NumberId);
            Assert.AreEqual(levelOverride.StartOverrideNumberingValue.Val.Value, result.StartValue);

            Assert.AreEqual(levelOverride.Level.NumberingFormat.Val.Value, result.NumberingFormat);
            Assert.AreEqual(levelOverride.Level.ParagraphStyleIdInLevel.Val.Value, result.ParagraphStyleId);
            Assert.AreEqual(levelOverride.Level.LevelText.Val.Value, result.Text);
        }

        [TestMethod]
        public void CreateFromNumbering_MixedTest2()
        {
            var (instance, numberingInstance, abstractNumConfig) = CreateNumberingInstanceContext();
            var levelOverrides = numberingInstance.Elements<LevelOverride>();
            var levelOverride = levelOverrides.Single(x => x.LevelIndex.Value == 4);

            var results = instance.CreateFromNumbering(abstractNumConfig, numberingInstance);
            var result = results.Single(x => x.LevelIndex == 4);
            var @abstract = abstractNumConfig.Single(x => x.LevelIndex == 4);

            Assert.AreEqual(levelOverride.LevelIndex.Value, result.LevelIndex);
            Assert.IsFalse(result.IsFromAbstract);

            Assert.AreEqual(@abstract.LevelIndex, result.LevelIndex);
            Assert.IsFalse(result.IsFromAbstract);
            Assert.AreEqual(@abstract.NumberingFormat, result.NumberingFormat);
            Assert.AreEqual(@abstract.ParagraphStyleId, result.ParagraphStyleId);
            Assert.AreEqual(@abstract.StartValue, result.StartValue);
            Assert.AreEqual(@abstract.Text, result.Text);
        }

        [TestMethod]
        public void CreateFromNumbering_MixedTest3()
        {
            var (instance, numberingInstance, abstractNumConfig) = CreateNumberingInstanceContext();
            var levelOverrides = numberingInstance.Elements<LevelOverride>();
            var levelOverride = levelOverrides.Single(x => x.LevelIndex.Value == 5);

            var results = instance.CreateFromNumbering(abstractNumConfig, numberingInstance);
            var result = results.Single(x => x.LevelIndex == 5);
            var @abstract = abstractNumConfig.Single(x => x.LevelIndex == 5);

            Assert.AreEqual(levelOverride.LevelIndex.Value, result.LevelIndex);
            Assert.IsFalse(result.IsFromAbstract);

            Assert.AreEqual(@abstract.LevelIndex, result.LevelIndex);
            Assert.IsFalse(result.IsFromAbstract);
            Assert.AreEqual(@abstract.NumberingFormat, result.NumberingFormat);
            Assert.AreEqual(@abstract.ParagraphStyleId, result.ParagraphStyleId);
            Assert.AreEqual(@abstract.StartValue, result.StartValue);
            Assert.AreEqual(@abstract.Text, result.Text);
        }
        private (ConfigFactory, NumberingInstance, Config) CreateNumberingInstanceContext()
        {
            var instance = new ConfigFactory();
            var numberingInstance = NumberingSample1.GenerateNumberingInstance();
            var abstractNumConfig = instance.CreateFromAbstractNumbering(NumberingSample1.GenerateAbstractNum());
            return (instance, numberingInstance, abstractNumConfig);
        }

        [TestMethod]
        public void CreateFromAbstractNumbering_NumStyleLinkTest()
        {
            var instance = new ConfigFactory();
            var abstractNum = NumberingSample2.GenerateAbstractNum18();

            Assert.ThrowsException<LinkedStyleNumberingException>(() =>
              instance.CreateFromAbstractNumbering(abstractNum));
        }

        [TestMethod]
        public void CreateFromAbstractNumbering_IsLegalTest()
        {
            var (instance, numberingInstance, abstractNumConfig) = CreateNumberingInstanceContext();

            var results = instance.CreateFromNumbering(abstractNumConfig, numberingInstance);
            var result = results.Single(x => x.LevelIndex == 8);

            Assert.IsTrue(result.ForceNumbericRendering);
        }
    }
}