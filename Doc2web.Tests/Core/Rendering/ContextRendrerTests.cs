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
            _elementContext.AddMultipleNodes(new HtmlNode[] // Force a node colisison
            {
                new HtmlNode { Start = 0, End = 5, Z = 1 },
                new HtmlNode { Start = 3, End = 7, Z = 0 },
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

            Assert.IsTrue(_elementContext.Nodes.Count() < result.Length);
        }

        //[TestMethod]
        //public void BuildTags_Test()
        //{
        //    var nodes = _elementContext.Nodes.ToArray();

        //    var tags = ContextRenderer.BuildTags(nodes);

        //    Assert.IsTrue(tags.Length > 0);
        //}

        //[TestMethod]
        //public void Render_Test() { }
        //{
        //    string expectedHtml = $"<p>{_elementContext.RootElement.InnerText}</p>";
        //    var m1 = Substitute.For<Mutation>();
        //    var m2 = Substitute.For<Mutation>();
        //    var openingTag = new OpeningTag { Index = 0, Name = "p" };
        //    var closingTag = new ClosingTag { Index = _elementContext.RootElement.InnerText.Length };
        //    closingTag.Related = openingTag;
        //    openingTag.Related = closingTag;

        //    var r = ContextRenderer.Render(
        //        _elementContext.RootElement.InnerText, 
        //        new Mutation[] { m1, m2 },
        //        new ITag[] { openingTag, closingTag });

        //    Assert.AreEqual(expectedHtml, r.ToString());
        //    m1.Received(1).MutateText(Arg.Any<StringBuilder>());
        //    m2.Received(1).MutateText(Arg.Any<StringBuilder>());
        //}
    }
}
