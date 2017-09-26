using Doc2web.Plugins.Numbering.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Tests.Plugins.Numbering.Mapping
{
    [TestClass]
    public class ConfigTests
    {
        [TestMethod]
        public void Render_FailTest()
        {
            var instance = BuildInstance();

            Assert.ThrowsException<ArgumentException>(() => instance.Render(Enumerable.Repeat(1, 100)));
        }

        [TestMethod]
        public void Render_Test1()
        {
            var instance = BuildInstance();
            var result = instance.Render(new int[] { 0 });

            Assert.AreEqual("ARTICLE - I", result);
        }

        [TestMethod]
        public void Render_Test2()
        {
            var instance = BuildInstance();
            var result = instance.Render(new int[] { 0, 0, 1, 1 });

            Assert.AreEqual("1.2 (b)", result);
        }

        [TestMethod]
        public void Render_ForceNumericRenderingTest()
        {
            var instance = BuildInstance();
            var result = instance.Render(new int[] { 0, 0, 0, 1, 0 });

            Assert.AreEqual("3.1", result);
        }

        private Config BuildInstance()
        {
            var instance = new Config();

            var iConfig1 = Substitute.For<ILevelConfig>();
            iConfig1.RenderNumber(Arg.Is(1)).Returns("I");
            iConfig1.Text.Returns("ARTICLE - %1");
            iConfig1.StartValue.Returns(1);

            var iConfig2 = Substitute.For<ILevelConfig>();
            iConfig2.RenderNumber(Arg.Is(1)).Returns("1");
            iConfig2.StartValue.Returns(1);

            var iConfig3 = Substitute.For<ILevelConfig>();
            iConfig3.RenderNumber(Arg.Is(2)).Returns("2");
            iConfig3.StartValue.Returns(1);

            var iConfig4 = Substitute.For<ILevelConfig>();
            iConfig4.RenderNumber(Arg.Is(3)).Returns("b");
            iConfig4.StartValue.Returns(2);
            iConfig4.Text.Returns("%2.%3 (%4)");

            var iConfig5 = Substitute.For<ILevelConfig>();
            iConfig5.RenderNumber(Arg.Is(1)).Returns("1");
            iConfig5.StartValue.Returns(1);
            iConfig5.Text.Returns("%4.%5");
            iConfig5.ForceNumbericRendering.Returns(true);

            instance.AddLevel(iConfig1);
            instance.AddLevel(iConfig2);
            instance.AddLevel(iConfig3);
            instance.AddLevel(iConfig4);
            instance.AddLevel(iConfig5);

            return instance;
        }
    }
}