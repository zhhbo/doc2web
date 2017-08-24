using Doc2web.Core;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Tests.Core
{
    [TestClass]
    public class ElementProcessingTaskTests
    {
        private INestableElementContext _nestableElementContext;
        private IProcessor _processor;
        private ElementProcessingTask _instance;

        [TestInitialize]
        public void Initialize()
        {
            _nestableElementContext = Substitute.For<INestableElementContext>();
            _nestableElementContext.Element.Returns(new Paragraph());
            _processor = Substitute.For<IProcessor>();
            _instance = new ElementProcessingTask(_nestableElementContext, _processor);
        }

        [TestMethod]
        public void ElementProcessingTask_Test()
        {
            _nestableElementContext.Received(1).NestingHandler = _instance;
        }

        [TestMethod]
        public void Execute_Test()
        {
            _instance.Execute();

            _processor.Received(1).ProcessElement(_nestableElementContext, _nestableElementContext.Element);
        }

        [TestMethod]
        public void QueueElementProcessing_Test()
        {
            var childContext = Substitute.For<INestableElementContext>();
            childContext.Element.Returns(new Paragraph());
            _processor
                .When(x => x.ProcessElement(Arg.Is(_nestableElementContext), Arg.Any<OpenXmlElement>()))
                .Do(x => {
                    for (int i = 0; i < 5; i++)
                        _instance.QueueElementProcessing(childContext);
                });

            _instance.Execute();

            _processor.Received(1).ProcessElement(childContext, childContext.Element);
        }

    }
}
