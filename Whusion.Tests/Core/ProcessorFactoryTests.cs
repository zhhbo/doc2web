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
        private IElementContext _elementContext;

        public class PreProcessorConfig
        {
            [PreProcessing]
            public virtual void PreProcess(IGlobalContext ctx) { }
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

        public class InvalidPreProcessingConfig
        {
            [PreProcessing]
            public virtual void PreProccess() { }
        }

        public class InvalidPostProcessorConfig
        {
            [PostProcessing]
            public virtual void PostProcess(object something) { }
        }

        public class InvalidElementProcessingConfig1
        {
            [ElementProcessing]
            public virtual void ProcessString(IElementContext ctx, string str) { }
        }

        public class InvalidElementProcessingConfig2
        {
            [ElementProcessing]
            public virtual void ProcessParagraph(IGlobalContext ctx, Paragraph p) { }
        }

        public class InvalidElementProcessingConfig3
        {
            [ElementProcessing]
            public virtual void ProcessRun(Run r) { }
        }

        [TestInitialize]
        public void Initalize()
        {
            _instance = new ProcessorFactory();
            _globalContext = Substitute.For<IGlobalContext>();
            _elementContext = Substitute.For<IElementContext>();
        }

        [TestMethod]
        public void BuildSingle_PreProcessing_Test()
        {
            var preProcessorConfig = BuildPreProcessorConfig();

            var processor = _instance.BuildSingle(preProcessorConfig);
            processor.PreProcess(_globalContext);

            Assert.AreEqual(1, processor.PreRenderingActions.Count);
            preProcessorConfig.Received(1).PreProcess(Arg.Is(_globalContext));
        }


        [TestMethod]
        public void BuildSingle_PostProcessing_Test()
        {
            var postProcessorConfig = BuildPostProcessorConfig();

            var processor = _instance.BuildSingle(postProcessorConfig);
            processor.PostProcess(_globalContext);

            Assert.AreEqual(1, processor.PostRenderingActions.Count);
            postProcessorConfig.Received(1).PostProcessing(Arg.Is(_globalContext));
        }

        [TestMethod]
        public void BuildSingle_ElementProcessing_Test()
        {
            var paragraph = new Paragraph();
            var elementProcessingConfig = BuildElementProcessorConfig();

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
        public void BuildSingle_PreProcessing_InvalidTest()
        {
            var preProcessorConfig = BuildInvalidPreProcessingConfig();

            Assert.ThrowsException<ArgumentException>(() =>
                _instance.BuildSingle(preProcessorConfig)
            );
        }

        [TestMethod]
        public void BuildSingle_PostProcessing_InvalidTest()
        {
            var postProcessingConfig = BuildInvalidPostProcessingConfig();

            Assert.ThrowsException<ArgumentException>(() =>
                _instance.BuildSingle(postProcessingConfig));
        }

        [TestMethod]
        public void BuildSingle_ElementProcessing_InvalidTest()
        {
            object[] processorConfigs = new object[]
            {
                new InvalidElementProcessingConfig1(),
                new InvalidElementProcessingConfig2(),
                new InvalidElementProcessingConfig3(),
            };

            foreach (object processorConfig in processorConfigs)
                Assert.ThrowsException<ArgumentException>(
                    () => _instance.BuildSingle(processorConfig)
                );
        }

        private PreProcessorConfig BuildPreProcessorConfig() =>
            Substitute.For<PreProcessorConfig>();

        private PostProcessorConfig BuildPostProcessorConfig() =>
            Substitute.For<PostProcessorConfig>();

        private ElementProcessorConfig BuildElementProcessorConfig() =>
            Substitute.For<ElementProcessorConfig>();

        private InvalidPreProcessingConfig BuildInvalidPreProcessingConfig() =>
            Substitute.For<InvalidPreProcessingConfig>();

        private InvalidPostProcessorConfig BuildInvalidPostProcessingConfig() =>
            Substitute.For<InvalidPostProcessorConfig>();

    }
}
