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
        private MemoryStream _stream;
        private ConversionTaskFactory _instance;
        private IContainer _engineContainer;
        private Processor _processor;
        private IContextRenderer _contextRenderer;

        [TestInitialize]
        public void Initialize()
        {
            _stream = new MemoryStream();
            _instance = new ConversionTaskFactory();
            _engineContainer = Substitute.For<IContainer>();
            _processor = new Processor();
            _contextRenderer = Substitute.For<IContextRenderer>();
            _instance.LifetimeScope = _engineContainer;
            _instance.Processor = _processor;
            _instance.ContextRenderer = _contextRenderer;
        }
        
        [TestCleanup]
        public void Cleanup()
        {
            _stream.Dispose();
        }

        [TestMethod]
        public void Build_Test()
        {
            var elements = new OpenXmlElement[]
            {
                new Paragraph(),
                new Paragraph()
            };

            var conversionTask = _instance.Build(elements, _stream) as ConversionTask;

            Assert.IsNotNull(conversionTask);
            Assert.AreSame(_engineContainer, conversionTask.LifetimeScope);
            Assert.AreSame(elements, conversionTask.RootElements);
            Assert.AreSame(_processor, conversionTask.Processor);
            Assert.AreSame(_contextRenderer, conversionTask.ContextRenderer);
            Assert.AreSame(_stream, conversionTask.Out.BaseStream);
        }

        [TestMethod]
        public void Build_TemporyPluginsTest()
        {
            var elements = new OpenXmlElement[] { };

            var tempProcessor = new Processor();
            tempProcessor.ElementRenderingActions.Add(typeof(Paragraph), new List<Action<IElementContext, OpenXmlElement>> { tempMethod });
            var conversionTask = _instance.Build(elements, _stream, tempProcessor) as ConversionTask;

            var finalProcessor = conversionTask.Processor as Processor; ;
            Assert.IsTrue(finalProcessor.ElementRenderingActions[typeof(Paragraph)].Contains(tempMethod));
        }

        private void tempMethod(IElementContext arg1, OpenXmlElement arg2)
        {
            throw new NotImplementedException();
        }
    }
}
