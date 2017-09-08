using Doc2web.Core.Rendering;
using Doc2web.Core.Rendering.Step3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Core.Rendering.Step3
{
    [TestClass]
    public class TagsRendererTest
    {
        private static (int, ITag)[] InputTags1 => new (int, ITag)[]
        {
            (1, new OpeningTag { Index=0 }),
            (0, new ClosingTag { Index = InputString1.Length })
        };
        private static string InputString1 = "This is a test.";
        private static string ExpectedString1 = "<div>This is a test.</div>";

        private static (int, ITag)[] InputTags2 => new (int, ITag)[]
        {
            (3, new OpeningTag { Index = 0 }),
            (2, new OpeningTag { Index = 10, Name = "span" }),
            (1, new ClosingTag { Index = 14 }),
            (0, new ClosingTag { Index = InputString2.Length })
        };
        private static string InputString2 = "This is a test.";
        private static string ExpectedString2 = "<div>This is a <span>test</span>.</div>";

        [TestMethod]
        public void RenderInto_Test1()
        {
            Test(InputTags1, InputString1, ExpectedString1);
        }

        [TestMethod]
        public void RenderInto_Test2()
        {
            Test(InputTags2, InputString2, ExpectedString2);
        }

        private void Test((int, ITag)[] tagsConfig, string input, string expected)
        {
            var tags = Utils.SetRelatedTag(tagsConfig);
            var stringBuilder = new StringBuilder(input);
            var result = TagsRenderer.Render(tags, stringBuilder);

            Assert.AreEqual(expected, result);
        }
    }
}
