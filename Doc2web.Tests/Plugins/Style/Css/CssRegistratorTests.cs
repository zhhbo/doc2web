using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Css;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Tests.Plugins.Style.Css
{
    [TestClass]
    public class CssRegistratorTests
    {
        private IParagraphClassFactory _paragraphClassFactory;
        private IRunClassFactory _runClassFactory;
        private CssRegistrator _instance;

        [TestInitialize]
        public void Initialize()
        {
            _paragraphClassFactory = Substitute.For<IParagraphClassFactory>();
            _runClassFactory = Substitute.For<IRunClassFactory>();
            _instance = new CssRegistrator(_paragraphClassFactory, _runClassFactory);
        }

        [TestMethod]
        public void CssRegistrator2_Test()
        {
            Assert.AreEqual(0, _instance.Registrations.Count());
        }

        [TestMethod]
        public void Register_NewParagraphTest()
        {
            var output = new CssClass();
            _paragraphClassFactory
                .Build(Arg.Any<ParagraphClassParam>())
                .Returns(output);
            var pPr = BuildPPr();

            var result = _instance.RegisterParagraph(pPr, (5, 6));

            Assert.AreSame(result, output);

            var param = ParagraphParams.Single();
            Assert.AreSame(pPr, param.InlineProperties);
            Assert.AreEqual("pstyle", param.StyleId);
            Assert.AreEqual(5, param.NumberingId);
            Assert.AreEqual(6, param.NumberingLevel);
            Assert.AreSame(result, _instance.Registrations.Single());
        }

        [TestMethod]
        public void Register_ExistingParagraph()
        {
            var props = new CssPropertiesSet {
                new MockProp1(),
                new MockProp2() 
            };
            _paragraphClassFactory
                .Build(Arg.Any<ParagraphClassParam>())
                .Returns(x => new CssClass { Props = props });

            var result1 = _instance.RegisterParagraph(BuildPPr());
            var result2 = _instance.RegisterParagraph(BuildPPr());

            Assert.AreSame(result1, result2);
            Assert.AreSame(result1, _instance.Registrations.Single());
        }

        [TestMethod]
        public void RegisterRun_NewTest()
        {
            var output = new CssClass();
            _runClassFactory
                .Build(Arg.Any<RunClassParam>())
                .Returns(output);
            var pPr = BuildPPr();
            var rPr = BuildRPr();

            var result = _instance.RegisterRun(pPr, rPr, (5, 6));

            Assert.AreSame(result, output);

            var param = RunParams.Single();
            Assert.AreEqual("pstyle", param.ParagraphStyleId);
            Assert.AreEqual("rstyle", param.RunStyleId);
            Assert.AreEqual(5, param.NumberingId);
            Assert.AreEqual(6, param.NumberingLevel);
            Assert.AreSame(rPr, param.InlineProperties);
            Assert.AreSame(result, _instance.Registrations.Single());
        }

        [TestMethod]
        public void Register_ExistingRun()
        {
            var props = new CssPropertiesSet { new MockProp1(), new MockProp2() };
            var output = new CssClass();
            _runClassFactory
                .Build(Arg.Any<RunClassParam>())
                .Returns(x => new CssClass { Props = props });

            var result1 = _instance.RegisterRun(BuildPPr(), BuildRPr());
            var result2 = _instance.RegisterRun(BuildPPr(), BuildRPr());

            Assert.AreSame(result1, result2);
            Assert.AreSame(result1, _instance.Registrations.Single());
        }

        [TestMethod]
        public void InsertCss_Test()
        {
            var expected = new CssData();
            expected.AddAttribute("span", "color", "red");
            expected.AddAttribute("p", "border", "1px solid black");

            var cls1 = new CssClass
            {
                Props = new CssPropertiesSet {
                    new MockProp1 { Out = ("span", "color", "red")}
                }
            };
            var cls2 = new CssClass
            {
                Props = new CssPropertiesSet
                {
                    new MockProp2 { Out = ("p", "border", "1px solid black")}
                }
            };
            _runClassFactory.Build(Arg.Any<RunClassParam>()).Returns(cls1);
            _paragraphClassFactory.Build(Arg.Any<ParagraphClassParam>()).Returns(cls2);
            _instance.RegisterRun(BuildPPr(), BuildRPr());
            _instance.RegisterParagraph(BuildPPr());

            var cssData = new CssData();
            _instance.InsertCss(cssData);

            Assert.AreEqual(expected, cssData);
        }

        private ParagraphClassParam[] ParagraphParams =>
            _paragraphClassFactory
            .ReceivedCalls()
            .Where(x => x.GetMethodInfo().Name == nameof(_paragraphClassFactory.Build))
            .Select(x => x.GetArguments()[0] as ParagraphClassParam)
            .ToArray();
        private RunClassParam[] RunParams =>
            _runClassFactory
            .ReceivedCalls()
            .Where(x => x.GetMethodInfo().Name == nameof(_runClassFactory.Build))
            .Select(x => x.GetArguments()[0] as RunClassParam)
            .ToArray();

        private ParagraphProperties BuildPPr() =>
            new ParagraphProperties
            {
                ParagraphStyleId = new ParagraphStyleId { Val = "pstyle" }
            };


        private RunProperties BuildRPr() =>
            new RunProperties
            {
                RunStyle = new RunStyle { Val = "rstyle" }
            };
    }
}
