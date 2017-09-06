using Doc2web.Core;
using Doc2web.Core.Rendering;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Tests.Core.Rendering
{
    [TestClass]
    public class ContextRendrerTests
    {
        private IElementContext _elementContext;

        [TestInitialize]
        public void Initialize()
        {
            _elementContext = new RootElementContext(
                Substitute.For<IGlobalContext>(),
                new Paragraph(
                    new Run(
                        new Text("This is the text")))
            );
            _elementContext.AddMultipleNodes(new HtmlNode[]
            {
                new HtmlNode { Start = 0, End = 10 },
                new HtmlNode { Start = 3, End = 7 },
            });
            _elementContext.AddMutations(new Mutation[]
            {
                Substitute.For<Mutation>(),
                Substitute.For<Mutation>()
            });
        }

        [TestMethod]
        public void BuildNodes_Test()
        {

            var result = ContextRenderer.BuildNodes(_elementContext);

            Assert.IsTrue(_elementContext.Nodes.Count() < result.Count);
            foreach (var m in _elementContext.Mutations)
                m.Received(1).MutateNodes(Arg.Any<List<HtmlNode>>());
        }

        [TestMethod]
        public void BuildTags_Test()
        {
            var nodes = _elementContext.Nodes.ToList();

            var tags = ContextRenderer.BuildTags(nodes);

            Assert.IsTrue(tags.Length > 0);
        }

        [TestMethod]
        public void Render_Test()
        {
            string expectedHtml = $"<p>{_elementContext.RootElement.InnerText}</p>"; 
            var openingTag = new OpeningTag { Index = 0, Name = "p" };
            var closingTag = new ClosingTag { Index = _elementContext.RootElement.InnerText.Length };
            closingTag.Related = openingTag;
            openingTag.Related = closingTag;

            var r = ContextRenderer.Render(_elementContext, new ITag[] { openingTag, closingTag });

            Assert.AreEqual(expectedHtml, r.ToString());
            foreach (var m in _elementContext.Mutations)
                m.Received(1).MutateText(Arg.Any<StringBuilder>());
        }
    }
}
