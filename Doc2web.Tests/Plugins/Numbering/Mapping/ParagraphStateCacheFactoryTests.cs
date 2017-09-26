using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class ParagraphStateCacheFactoryTests
    {
        [TestMethod]
        public void FindNubmerindId_NestedElementTest()
        {
            var instance = new ParagraphStateCacheFactory(
              null,
              null
            );
            var p = DocumentSample1.GenerateParagraph1();

            var result = instance.FindAssociatedNumberingInstance(p);

            Assert.AreEqual(1, result.Value);
        }

        [TestMethod]
        public void FindPNumberingId_StyleTest()
        {
            var instance = new ParagraphStateCacheFactory(
              DocumentSample1.GenerateStyles(),
              null
            );
            var p = DocumentSample1.GenerateParagraph2();

            var result = instance.FindAssociatedNumberingInstance(p);

            Assert.AreEqual(1, result.Value);
        }

        [TestMethod]
        public void FindPNumbringStyleID_BaseOnStyleTest()
        {
            var instance = new ParagraphStateCacheFactory(
              DocumentSample1.GenerateStyles(),
              null
            );
            var p = DocumentSample1.GenerateParagraph3();

            var result = instance.FindAssociatedNumberingInstance(p);

            Assert.AreEqual(1, result.Value);
        }

        [TestMethod]
        public void FindPNumberingLevel_NoProperties()
        {
            var instance = new ParagraphStateCacheFactory(
              null,
              null
            );
            var p = new Paragraph();

            var result = instance.FindAssociatedNumberingLevel(p);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void FindPNumberingLevel_NestedElementTest()
        {
            var instance = new ParagraphStateCacheFactory(null, null);
            var p = DocumentSample1.GenerateParagraph1();

            var result = instance.FindAssociatedNumberingLevel(p);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void FindPNumberingLevel_StyleTest()
        {
            var instance = new ParagraphStateCacheFactory(
              DocumentSample1.GenerateStyles(),
              null
            );
            var p = DocumentSample1.GenerateParagraph2();

            var result = instance.FindAssociatedNumberingLevel(p);

            Assert.AreEqual(0, result);
        }


        [TestMethod]
        public void FindPNumberingLevel_BasedOnStyleTest()
        {
            var instance = new ParagraphStateCacheFactory(
              DocumentSample1.GenerateStyles(),
              null
            );
            var p = DocumentSample1.GenerateParagraph3();

            var result = instance.FindAssociatedNumberingLevel(p);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CreateCache_Test()
        {
            var body = DocumentSample1.GenerateBody();
            var instance = new ParagraphStateCacheFactory(
              DocumentSample1.GenerateStyles(),
              body
            );

            var cache = instance.Create();

            var ps1 = body.Elements<Paragraph>().Take(3);
            for (int i = 0; i < ps1.Count(); i++)
            {
                var p = ps1.ElementAt(i);
                var r = cache.Get(p);
                Assert.AreEqual(1, r.NumberingInstanceId);
                Assert.AreEqual(i, r.Indentations.Single());
            }

            var ps2 = body.Elements<Paragraph>().Skip(6);
            for (int i = 0; i < ps2.Count(); i++)
            {
                var p = ps2.ElementAt(i);
                var r = cache.Get(p);
                Assert.AreEqual(2, r.NumberingInstanceId);
                Assert.AreEqual(i, r.Indentations.Single());
            }
        }

        [TestMethod]
        public void CreateCache_StairsTest()
        {
            var body = DocumentSample2.GenerateBody();
            var instance = new ParagraphStateCacheFactory(null, body);
            var cache = instance.Create();

            var ps = body.Elements<Paragraph>().ToArray();
            var results = ps.Select(cache.Get).ToArray();

            var mm = results.Select(x => (x.Indentations.Count(), x.Indentations.Last())).ToArray();

            for (int i = 0; i < results.Length; i++)
            {
                var textSplits = ps[i].InnerText.Split(' ').Select(int.Parse).ToArray();
                var expectedLevel = textSplits[0];
                var expectedLastVal = textSplits[1];

                Assert.AreEqual(expectedLevel, results[i].Indentations.Count());
                Assert.AreEqual(expectedLastVal, results[i].Indentations.Last());
            }
        }

        [TestMethod]
        public void CreateCache_SkipTest()
        {
            var p = DocumentSample2.GenerateParagraph(1, "test", true);
            var instance = new ParagraphStateCacheFactory(null, null);

            Assert.IsNull(instance.FindAssociatedNumberingInstance(p));
        }
    }
}