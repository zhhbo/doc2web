using Autofac;
using Doc2web.Plugins.Numbering;
using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Properties;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Style
{
    [TestClass]
    public class CssPropertiesFactoryTests
    {
        private ILifetimeScope lifetimeScope;
        private CssPropertiesFactory instance;

        private static ILifetimeScope BuildContainer()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<StyleConfiguration>();
            containerBuilder.RegisterType<NumberingPluginConfig>();
            containerBuilder
                .RegisterType<BoldCssProperty>()
                .As<CssProperty<Bold>>();
            containerBuilder
                .RegisterType<IdentationCssProperty>()
                .As<CssProperty<Indentation>>();
            containerBuilder
                .RegisterType<NumberingIndentationCssProperty>()
                .As<CssProperty<Indentation>>();
            var container = containerBuilder.Build();
            var lifetimeScope = container.BeginLifetimeScope();
            return lifetimeScope;
        }

        [TestInitialize]
        public void Initialize()
        {
            lifetimeScope = BuildContainer();
            instance = new CssPropertiesFactory(lifetimeScope);

        }

        [TestMethod]
        public void BuildRun_Test()
        {
            var element = new RunProperties(
                new Bold { Val = new OnOffValue(true) }
            );

            var result = instance.BuildRun(element);

            Assert.AreEqual(1, result.Length);
            Assert.IsInstanceOfType(result[0], typeof(BoldCssProperty));
            Assert.AreSame(result[0].OpenXmlElement, element.Bold);
        }

        [TestMethod]
        public void BuildParagraph_Test()
        {
            var element = new ParagraphProperties(
                new Indentation()
            );

            var result = instance.BuildParagraph(element);

            Assert.AreEqual(1, result.Length);
            Assert.IsInstanceOfType(result[0], typeof(IdentationCssProperty));
            Assert.AreSame(result[0].OpenXmlElement, element.Indentation);
        }


        [TestMethod]
        public void BuildNumbering_Test()
        {
            var element = new PreviousParagraphProperties(
                new Indentation()
            );

            var result = instance.BuildNumbering(element);

            Assert.AreEqual(1, result.Length);
            Assert.IsInstanceOfType(result[0], typeof(NumberingIndentationCssProperty));
            Assert.AreSame(result[0].OpenXmlElement, element.Indentation);
        }
    }
}
