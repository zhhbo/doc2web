using Doc2web.Core.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Tests.Core.Rendering
{
    public static class Utils
    {

        /// <summary>
        /// Set the "Related" property of an list of tags.
        /// </summary>
        /// <param name="expectedConfig">First is the index of the related, seccond is the tag.</param>
        /// <returns>Array containing the same tags.</returns>
        public static ITag[] SetRelatedTag((int, ITag)[] expectedConfig)
        {
            var results = expectedConfig.Select(x => x.Item2).ToArray();

            for (int i = 0; i < results.Length; i++)
            {
                var related = results[expectedConfig[i].Item1];
                var upgradeTarget = results[i];
                AssociateRelated(related, upgradeTarget);
            }

            return results;
        }

        public static void AssociateRelated(ITag related, ITag upgradeTarget)
        {
            switch (upgradeTarget)
            {
                case OpeningTag t:
                    t.Related = (ClosingTag)related;
                    break;

                case ClosingTag t:
                    t.Related = (OpeningTag)related;
                    break;

                default: break;
            }
        }

        public static void AssertTagsArraysAreEquals(ITag[] expected, ITag[] result)
        {
            Assert.AreEqual(expected.Length, result.Length);

            for (int i = 0; i < result.Length; i++)
            {
                var r = result[i];
                var e = expected[i];

                Utils.AssertTagsAreEquals(e, r);
            }
        }

        public static void AssertTagsAreEquals(ITag e, ITag r)
        {
            switch (e)
            {
                case SelfClosingTag selfClosingE when r is SelfClosingTag:
                    AssertSelfClosingTagAreEqual(selfClosingE, (SelfClosingTag)r);
                    break;

                case OpeningTag openingE when r is OpeningTag:
                    AssertOpeningTagsAreEqual(openingE, (OpeningTag)r);
                    break;

                case ClosingTag closingE when r is ClosingTag:
                    AssertClosingTagsAreEqual(closingE, (ClosingTag)r);
                    break;

                default:
                    Assert.Fail("Tags are not the same type");
                    break;
            }
        }

        private static void AssertSelfClosingTagAreEqual(SelfClosingTag expected, SelfClosingTag result)
        {
            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.Position, result.Position);
            AssertAttributesAreEquals(expected.Attributes, result.Attributes);
        }

        private static void AssertOpeningTagsAreEqual(OpeningTag expected, OpeningTag result)
        {
            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.Position, result.Position);
            Assert.AreEqual(expected.Attributes, result.Attributes);
            Assert.AreEqual(expected.TextAfter, result.TextAfter);
            AssertAttributesAreEquals(expected.Attributes, result.Attributes);
            AssertClosingTagsAreEqual(expected.Related, result.Related);
        }

        private static void AssertClosingTagsAreEqual(ClosingTag expected, ClosingTag result)
        {
            Assert.AreEqual(expected.Position, result.Position);
            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.RelatedPosition, result.RelatedPosition);
            Assert.AreEqual(expected.TextBefore, result.TextBefore);
        }

        private static void AssertAttributesAreEquals
            (IReadOnlyDictionary<string, string> expected, IReadOnlyDictionary<string, string> result)
        {
            Assert.AreEqual(expected.Keys.ToArray(), result.Keys.ToArray());
            Assert.AreEqual(expected.Values.ToArray(), result.Values.ToArray());
        }

    }
}
