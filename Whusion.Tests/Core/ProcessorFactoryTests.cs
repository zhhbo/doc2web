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

        [TestMethod]
        public void BuildSingle_PreProcessing_Test()
        {
            var instance = new ProcessorFactory();
            var globalContext = Substitute.For<IGlobalContext>();
            var preProcessorConfig = BuildPreProcessorConfig();

            var processor = instance.BuildSingle(preProcessorConfig);
            Assert.AreEqual(1, processor.PreRendering.Count);

            processor.PreRendering[0](globalContext);
            preProcessorConfig.Received(1).PreProcess(Arg.Is(globalContext));
        }

        [TestMethod]
        public void BuildSingle_PostProcessing_Test()
        {
            var instance = new ProcessorFactory();
            var globalContext = Substitute.For<IGlobalContext>();
            var postProcessorConfig = BuildPostProcessorConfig();

            var processor = instance.BuildSingle(postProcessorConfig);
            Assert.AreEqual(1, processor.PostRendering.Count);

            processor.PostRendering[0](globalContext);
            postProcessorConfig.Received(1).PostProcessing(Arg.Is(globalContext));
        }

        [TestMethod]
        public void BuildSingle_ElementProcessing_Test()
        {
            var instance = new ProcessorFactory();
            var elementContext = Substitute.For<IElementContext>();
            var paragraph = new Paragraph();
            var elementProcessingConfig = BuildElementProcessorConfig();

            var processor = instance.BuildSingle(elementProcessingConfig);
            Assert.AreEqual(
                1,
                processor.ElementRendering[typeof(Paragraph)].Count
            );

            processor.ElementRendering[typeof(Paragraph)][0](elementContext, paragraph);
            elementProcessingConfig
                .Received(1)
                .ProcessElement(Arg.Is(elementContext), Arg.Is(paragraph));
        }

        [TestMethod]
        public void BuildSingle_PreProcessing_InvalidTest()
        {
            var instance = new ProcessorFactory();
            var globalContext = Substitute.For<IGlobalContext>();
            var preProcessorConfig = BuildInvalidPreProcessingConfig();

            Assert.ThrowsException<ArgumentException>(() =>
                instance.BuildSingle(preProcessorConfig)
            );
        }

        [TestMethod]
        public void BuildSingle_PostProcessing_InvalidTest()
        {
            var instance = new ProcessorFactory();
            var globalContext = Substitute.For<IGlobalContext>();
            var postProcessingConfig = BuildInvalidPostProcessingConfig();

            Assert.ThrowsException<ArgumentException>(() =>
                instance.BuildSingle(postProcessingConfig));
        }

        [TestMethod]
        public void BuildSingle_ElementProcessing_InvalidTest()
        {
            var processorFactory = new ProcessorFactory();
            object[] instances = new object[]
            {
                new InvalidElementProcessingConfig1(),
                new InvalidElementProcessingConfig2(),
                new InvalidElementProcessingConfig3(),
            };
            foreach (object instance in instances)
                Assert.ThrowsException<ArgumentException>(
                    () => processorFactory.BuildSingle(instance)
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
