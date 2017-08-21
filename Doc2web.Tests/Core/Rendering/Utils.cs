using Doc2web.Core.Rendering;
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


    }
}
