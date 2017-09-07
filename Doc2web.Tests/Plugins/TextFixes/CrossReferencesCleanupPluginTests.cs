using Doc2web.Core;
using Doc2web.Plugins.TextFixes;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Tests.Plugins.TextFixes
{
    [TestClass]
    public class CrossReferencesCleanupPluginTests
    {
        private static string text = @"“Vendor” has the meaning ascribed thereto in Sections  REF _Ref459698240 \r \h \* MERGEFORMAT 6.5.7, REF _Ref459698276 \r \h \* MERGEFORMAT 6.6.7 or REF _Ref459698295 \r \h \* MERGEFORMAT 7.5 hereof, as the context requires;";

        [TestMethod]
        public void RemoveCrossRefs_Test()
        {
            var p = new Paragraph(new Run(new Text(text)));
            var context = new RootElementContext(null, p);
            var instance = new CrossReferencesCleanupPlugin();

            instance.RemoveCrossRefs(context, p);

            var mutations = context.Mutations
                .Select(x => x as TextDeletion)
                .Select(x => (x.Index, x.Length))
                .ToArray();

            Assert.AreEqual(3, mutations.Length);
            Assert.AreEqual((055, 39), mutations[0]);
            Assert.AreEqual((101, 39), mutations[1]);
            Assert.AreEqual((149, 39), mutations[2]);
        }
    }
}
