using Doc2web.Core.Rendering;
using Doc2web.Core.Rendering.Step3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Tests.Core.Rendering
{
    [TestClass]
    public class Rendering2Tests
    {


        [TestMethod]
        public void Render_OffLimitsTests()
        {
            var r = new Renderer2()
            {
                Text = "Something.",
                Elements = new IRenderable[]
                {
                    BuildSingle(double.MinValue, 0, "<div>"),
                    BuildSingle(double.MaxValue, 0, "</div>"),
                }
            };

            Assert.AreEqual("<div>Something.</div>", r.Render());
        }

        [TestMethod]
        public void Render_MultipleTests()
        {
            var r = new Renderer2
            {
                Text = "Some doggy bag",
                Elements = Build(
                    (double.MinValue, 0, "<div>"),
                    (05, 0, "<span>"),
                    (10, 0, "</span>"),
                    (double.MaxValue, 0, "</div>")
                )
            };
            Assert.AreEqual("<div>Some <span>doggy</span> bag</div>", r.Render());
        }

        [TestMethod]
        public void Render_OffsetTests()
        {
            var t = "Something is gone.";
            var w = "gone";
            var r = new Renderer2()
            {
                Text = t,
                Elements = new IRenderable[]
                {
                    BuildSingle(t.IndexOf(w), w.Length, ""),
                }
            };

            Assert.AreEqual("Something is .", r.Render());
        }

        [TestMethod]
        public void Render_OverlapseOffsetTest()
        {
            var t = "Something is gone.";
            var w = "gone";
            var r = new Renderer2()
            {
                Text = t,
                Elements = new IRenderable[]
                {
                    BuildSingle(t.IndexOf(w), w.Length, "not gone"),
                    BuildSingle(t.IndexOf(w), 0, " here"),
                }
            };

            Assert.AreEqual("Something is not gone here.", r.Render());
        }

        private IRenderable[] Build(params (double, double, string)[] elems) =>
            elems
            .Select(x => BuildSingle(x.Item1, x.Item2, x.Item3))
            .ToArray();

        private IRenderable BuildSingle(double pos, double offset, string text)
        {
            var r = Substitute.For<IRenderable>();
            r.Position.Returns(pos);
            r.Offset.Returns(offset);
            r.Render().Returns(text);
            return r;
        }

    }
}
