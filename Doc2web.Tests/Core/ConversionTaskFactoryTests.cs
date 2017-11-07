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
using System.IO;

namespace Doc2web.Tests.Core
{
    [TestClass]
    public class ConversionTaskFactoryTests
    {
        private ConversionParameter _parameter;
        private ConversionTaskFactory _instance;

        [TestInitialize]
        public void Initialize()
        {
            _parameter = new ConversionParameter
            {
                Stream = new MemoryStream()
            };
            _instance = new ConversionTaskFactory();
            _instance.LifetimeScope = Substitute.For<IContainer>();
            _instance.Processor = new Processor();
            _instance.ContextRenderer = Substitute.For<IContextRenderer>();
            _instance.ProcessorFactory = Substitute.For<IProcessorFactory>();
        }
        
        [TestCleanup]
        public void Cleanup()
        {
            _parameter.Stream.Dispose();
        }

        [TestMethod]
        public void Build_Test()
        {
            _parameter.Elements = new OpenXmlElement[]
            {
                new Paragraph(),
                new Paragraph()
            };

            var conversionTask = _instance.Build(_parameter) as ConversionTask;

            Assert.IsNotNull(conversionTask);
            Assert.AreSame(_parameter.Elements, conversionTask.RootElements);
            Assert.AreSame(_parameter.Stream, conversionTask.Out.BaseStream);
            Assert.AreSame(_instance.LifetimeScope, conversionTask.LifetimeScope);
            Assert.AreSame(_instance.Processor, conversionTask.Processor);
            Assert.AreSame(_instance.ContextRenderer, conversionTask.ContextRenderer);
        }

        [TestMethod]
        public void Build_TemporyPluginsTest()
        {
            var elements = new OpenXmlElement[] { };
            var tempPlugin = new object();
            _parameter.AdditionalPlugins.Add(tempPlugin);

            var tempProcessor = new Processor();
            tempProcessor.ElementRenderingActions.Add(typeof(Paragraph), new List<Action<IElementContext, OpenXmlElement>> { tempMethod });
            _instance.ProcessorFactory
                .BuildMultiple(Arg.Any<object[]>())
                .Returns(tempProcessor);


            var conversionTask = _instance.Build(_parameter) as ConversionTask;

            var finalProcessor = conversionTask.Processor as Processor; ;
            Assert.IsTrue(finalProcessor.ElementRenderingActions[typeof(Paragraph)].Contains(tempMethod));
            _instance.ProcessorFactory.ReceivedWithAnyArgs().BuildMultiple();
        }

        [TestMethod]
        public void Build_AutoFlushTest()
        {
            _parameter.Elements = new OpenXmlElement[]
            {
                new Paragraph(),
                new Paragraph()
            };
            _parameter.AutoFlush = true;

            var conversionTask = _instance.Build(_parameter) as ConversionTask;

            Assert.IsTrue(conversionTask.Out.AutoFlush);
        }

        private void tempMethod(IElementContext arg1, OpenXmlElement arg2)
        {
        }
    }
}
