using Autofac;
using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Css;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Plugins.Style
{
    [TestClass]
    public class StylePluginTests
    {
        private StylePlugin _instance;

        [TestInitialize]
        public void Initialize()
        {
            _instance = new StylePlugin(Samples.NumberingSample3.BuildDoc());
        }

        [TestMethod]
        public void InitializeEngine_Test()
        {
            var containerBuilder = new ContainerBuilder();

            _instance.InitEngine(containerBuilder);
            var container = containerBuilder.Build();
            var registrator = container.Resolve<ICssRegistrator>();

            Assert.IsNotNull(registrator);
        }

        [TestMethod]
        public void InjectCss_Test()
        {
            var cssRegistrator = Substitute.For<ICssRegistrator>();
            cssRegistrator
                .When(x =>x.InsertCss(Arg.Any<CssData>()))
                .Do(x => InsertCss(x.ArgAt<CssData>(0)));
            var context = Substitute.For<IGlobalContext>();
            context.Resolve<ICssRegistrator>().Returns(cssRegistrator);

            _instance.InjectCss(context);

            context.Received(1).AddCss(_instance.BaseCss);
            context.Received(1).AddCss("span {color: red;}");
        }

        private void InsertCss(CssData cssData)
        {
            cssData.AddAttribute("span", "color", "red");
        }
    }
}
