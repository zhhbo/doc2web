using Doc2web.Plugins.Style.Properties;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Doc2web.Plugins.Style;
using Autofac;
using DocumentFormat.OpenXml;

namespace Doc2web.Tests.Plugins.Style
{
    [TestClass]
    public class CssPropertiesFactoryTests
    {
        private IContainer _container;
        private CssPropertiesFactory _paragraphFac;
        private CssPropertiesFactory _runFac;

        public class MockBaseBoldCssProp : CssProperty<Bold>
        {
            public override short GetSpecificHashcode()
            {
                throw new NotImplementedException();
            }

            public override bool HaveSameOuput(Bold element)
            {
                throw new NotImplementedException();
            }

            public override void InsertCss(CssData cssData)
            {
                throw new NotImplementedException();
            }
        }

        public class MockBaseIdentationCssProp : CssProperty<Indentation>
        {
            public override short GetSpecificHashcode()
            {
                throw new NotImplementedException();
            }

            public override bool HaveSameOuput(Indentation element)
            {
                throw new NotImplementedException();
            }

            public override void InsertCss(CssData cssData)
            {
                throw new NotImplementedException();
            }
        }

        [ParagraphCssProperty(typeof(Bold))]
        public class ParagraphBoldCssProp : MockBaseBoldCssProp { }

        [RunCssProperty(typeof(Bold))]
        public class RunBoldCssProp : MockBaseBoldCssProp { }

        [NumberingCssProperty(typeof(Bold))]
        public class NumberingBoldCssProp : MockBaseBoldCssProp { }

        [ParagraphCssProperty(typeof(Indentation))]
        public class ParagraphIndentCssProp : MockBaseIdentationCssProp { }

        [RunCssProperty(typeof(Indentation))]
        public class RunIndentCssProp : MockBaseIdentationCssProp { }

        [NumberingCssProperty(typeof(Indentation))]
        public class NumberingIndentCssProp : MockBaseIdentationCssProp { }

        [TestInitialize]
        public void Initialize()
        {
            _container = BuildContainer();
            var fac = _container
                .Resolve<Func<CssPropertySource, CssPropertiesFactory>>();

            _paragraphFac = fac(CssPropertySource.Paragraph);
            _runFac = fac(CssPropertySource.Run);
        }

        private IContainer BuildContainer()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder
                .RegisterTypes(
                    typeof(ParagraphBoldCssProp),
                    typeof(RunBoldCssProp),
                    typeof(NumberingBoldCssProp),
                    typeof(ParagraphIndentCssProp),
                    typeof(RunIndentCssProp),
                    typeof(NumberingIndentCssProp)
                )
                .WithMetadataFrom<BaseCssPropertyAttribute>()
                .As<ICssProperty>();

            containerBuilder.RegisterType<CssPropertiesFactory>();

            return containerBuilder.Build();
        }

        [TestMethod]
        public void CssPropertyFactory_SingleTest()
        {
            var b = new Bold();

            var result = _paragraphFac.Build(b);

            Assert.AreEqual(1, result.Length);
            Assert.IsInstanceOfType(result[0], typeof(ParagraphBoldCssProp));
        }

        [TestMethod]
        public void CssPropertyFactory_MultipleTest()
        {
            var props = new RunProperties(
                new Bold(),
                new Indentation()
            );

            var result = _runFac.Build(props);

            Assert.AreEqual(2, result.Length);
            Assert.IsInstanceOfType(result[0], typeof(RunBoldCssProp));
            Assert.IsInstanceOfType(result[1], typeof(RunIndentCssProp));
        }
    }
}
