using Autofac;
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
        private static ILifetimeScope BuildContainer()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder
                .RegisterType<BoldCssProperty>()
                .As<CssProperty<Bold>>();
            containerBuilder
                .RegisterType<CapsCssProperty>()
                .As<CssProperty<Caps>>();
            var container = containerBuilder.Build();
            var lifetimeScope = container.BeginLifetimeScope();
            return lifetimeScope;
        }

        private RunProperties BuildRunProperties() =>
            new RunProperties(
                new Bold { Val = new OnOffValue(true) },
                new Caps());

        [TestMethod]
        public void Build_Test()
        {
            var lifetimeScope = BuildContainer();
            var element = BuildRunProperties();
            var chilElements = element.ChildElements;
            var instance = new CssPropertiesFactory(lifetimeScope);

            var cssProps = instance.Build(element);

            Assert.AreEqual(2, cssProps.Length);

            Assert.IsInstanceOfType(cssProps[0], typeof(BoldCssProperty));
            Assert.IsInstanceOfType(cssProps[1], typeof(CapsCssProperty));

            for (var i = 0; i < chilElements.Count; i++)
                Assert.AreSame(chilElements[i], cssProps[i].OpenXmlElement);
        }

    }
}
