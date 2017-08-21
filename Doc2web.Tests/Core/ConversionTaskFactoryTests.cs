using Autofac;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using Doc2web.Core;
using Doc2web.Core.Rendering;

namespace Doc2web.Tests.Core
{
    [TestClass]
    public class ConversionTaskFactoryTests
    {
        private ConversionTaskFactory _instance;
        private IContainer _engineContainer;
        private ILifetimeScope _taskContainer;
        private IProcessor _processor;
        private IContextRenderer _contextRenderer;

        [TestInitialize]
        public void Initialize()
        {
            _instance = new ConversionTaskFactory();
            _engineContainer = Substitute.For<IContainer>();
            _taskContainer = Substitute.For<ILifetimeScope>();
            _engineContainer.BeginLifetimeScope().Returns(_taskContainer);
            _processor = Substitute.For<IProcessor>();
            _contextRenderer = Substitute.For<IContextRenderer>();
            _instance.EngineContainer = _engineContainer;
            _instance.Processor = _processor;
            _instance.ContextRenderer = _contextRenderer;
        }

        [TestMethod]
        public void Build_Test()
        {
            var elements = new OpenXmlElement[]
            {
                new Paragraph(),
                new Paragraph()
            };

            var conversionTask = _instance.Build(elements) as ConversionTask;

            Assert.IsNotNull(conversionTask);
            Assert.AreSame(_taskContainer, conversionTask.GlobalContext.Container);
            Assert.AreSame(elements, conversionTask.GlobalContext.RootElements);
            Assert.AreSame(_processor, conversionTask.Processor);
            Assert.AreSame(_contextRenderer, conversionTask.ContextRenderer);
        }

    }
}
