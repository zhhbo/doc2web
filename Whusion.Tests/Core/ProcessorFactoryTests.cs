using Autofac;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using Whusion.Core;

namespace Whusion.Tests.Core
{
    [TestClass]
    public class ProcessorFactoryTests
    {
        private ProcessorFactory _instance;
        private IGlobalContext _globalContext;
        private ContainerBuilder _containerBuilder;
        private IElementContext _elementContext;

        public class PreProcessorConfig
        {
            [PreProcessing]
            public virtual void PreProcess(IGlobalContext ctx, ContainerBuilder containerBuilder) { }
        }

        public class PostProcessorConfig
        {
            [PostProcessing]
            public virtual void PostProcessing(IGlobalContext ctx) { }
        }

        public class ElementProcessorConfig
        {
            [ElementProcessing]
            public virtual void ProcessElement(IElementContext context, Paragraph p) { }
        }

        public class InvalidPreProcessorConfig1
        {
            [PreProcessing]
            public virtual void PreProccess(IGlobalContext ctx) { }
        }

        public class InvalidPreProcessorConfig2
        {
            [PreProcessing]
            public virtual void PreProccess(IGlobalContext ctx, object something) { }
        }

        public class InvalidPostProcessorConfig1
        {
            [PostProcessing]
            public virtual void PostProcess() { }
        }

        public class InvalidPostProcessorConfig2
        {
            [PostProcessing]
            public virtual void PostProcess(object something) { }
        }

        public class InvalidElementProcessorConfig1
        {
            [ElementProcessing]
            public virtual void ProcessString(IElementContext ctx, string str) { }
        }

        public class InvalidElementProcessorConfig2
        {
            [ElementProcessing]
            public virtual void ProcessParagraph(IGlobalContext ctx, Paragraph p) { }
        }

        public class InvalidElementProcessorConfig3
        {
            [ElementProcessing]
            public virtual void ProcessRun(Run r) { }
        }

        [TestInitialize]
        public void Initalize()
        {
            _instance = new ProcessorFactory();
            _containerBuilder = new ContainerBuilder();
            _globalContext = Substitute.For<IGlobalContext>();
            _elementContext = Substitute.For<IElementContext>();
        }

        [TestMethod]
        public void BuildSingle_PreProcessing_Test()
        {
            var preProcessorConfig = Substitute.For<PreProcessorConfig>();

            var processor = _instance.BuildSingle(preProcessorConfig);
            processor.PreProcess(_globalContext, _containerBuilder);

            Assert.AreEqual(1, processor.PreRenderingActions.Count);
            preProcessorConfig.Received(1).PreProcess(_globalContext, _containerBuilder);
        }

        [TestMethod]
        public void BuildSingle_PreProcessing_InvalidTest()
        {
            var preProcessorConfigs = new object[]
            {
                Substitute.For<InvalidPreProcessorConfig1>(),
                Substitute.For<InvalidPreProcessorConfig2>()
            };

            foreach (var preProcessorConfig in preProcessorConfigs)
                Assert.ThrowsException<ArgumentException>(() =>
                    _instance.BuildSingle(preProcessorConfig));
        }


        [TestMethod]
        public void BuildSingle_PostProcessing_Test()
        {
            var postProcessorConfig = Substitute.For<PostProcessorConfig>();

            var processor = _instance.BuildSingle(postProcessorConfig);
            processor.PostProcess(_globalContext);

            Assert.AreEqual(1, processor.PostRenderingActions.Count);
            postProcessorConfig.Received(1).PostProcessing(_globalContext);
        }

        [TestMethod]
        public void BuildSingle_PostProcessing_InvalidTest()
        {
            var postProcessorConfigs = new object[]
            {
                new InvalidPostProcessorConfig1(),
                new InvalidPostProcessorConfig2()
            };

            foreach (var postProcessorConfig in postProcessorConfigs)
                Assert.ThrowsException<ArgumentException>(() =>
                    _instance.BuildSingle(postProcessorConfig));
        }

        [TestMethod]
        public void BuildSingle_ElementProcessing_Test()
        {
            var paragraph = new Paragraph();
            var elementProcessingConfig = Substitute.For<ElementProcessorConfig>();

            var processor = _instance.BuildSingle(elementProcessingConfig);
            processor.ProcessElement(_elementContext, paragraph);

            Assert.AreEqual(
                1,
                processor.ElementRenderingActions[typeof(Paragraph)].Count
            );
            elementProcessingConfig
                .Received(1)
                .ProcessElement(Arg.Is(_elementContext), Arg.Is(paragraph));
        }


        [TestMethod]
        public void BuildSingle_ElementProcessing_InvalidTest()
        {
            object[] processorConfigs = new object[]
            {
                new InvalidElementProcessorConfig1(),
                new InvalidElementProcessorConfig2(),
                new InvalidElementProcessorConfig3(),
            };

            foreach (object processorConfig in processorConfigs)
                Assert.ThrowsException<ArgumentException>(
                    () => _instance.BuildSingle(processorConfig)
                );
        }

    }
}
