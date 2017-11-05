using Autofac;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Doc2web.Core;
using System.IO;

namespace Doc2web.Tests.Core
{
    [TestClass]
    public class ConversionTaskTests
    {
        private ConversionTask _instance;
        private MemoryStream _stream;
        private IGlobalContext _globalContext;
        private IContainer _engineContainer;
        private RootElementContext[] _rootElementContext;
        private IProcessor _processor;
        private IContextRenderer _contextRenderer;

        [TestInitialize]
        public void Initialize()
        {
            _stream = new MemoryStream();
            _globalContext = Substitute.For<IGlobalContext>();
            _engineContainer = Substitute.For<IContainer>();
            _rootElementContext = new RootElementContext[] { 
                new RootElementContext(_globalContext, new Paragraph()),
                new RootElementContext(_globalContext, new Paragraph()),
            };
            _globalContext.RootElements.Returns(_rootElementContext);
            _contextRenderer = Substitute.For<IContextRenderer>();
            _contextRenderer.Render(_rootElementContext[0]).Returns(@"<p>1</p>");
            _contextRenderer.Render(_rootElementContext[1]).Returns(@"<p>2</p>");
            _processor = Substitute.For<IProcessor>();

            BuildInstance();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _stream.Dispose();
        }

        private void BuildInstance()
        {
            _instance = new ConversionTask
            {
                GlobalContext = _globalContext,
                Processor = _processor,
                ContextRenderer = _contextRenderer,
                LifetimeScope = _engineContainer,
                RootElements = _rootElementContext.Select(x => x.RootElement),
                Out = new StreamWriter(_stream)
            };
        }

        [TestMethod]
        public void PreProcess_Test()
        {
            _instance.GlobalContext = null;
            _instance.PreProcess();

            Assert.IsNotNull(_instance.GlobalContext);
            _processor.Received(1).PreProcess(_instance.GlobalContext);
        }

        [TestMethod]
        public void ConvertElements_Test()
        {
            _instance.ProcessElements();

            AssertHasProcessedAllRootElements();
        }

        [TestMethod]
        public void PostProcess_Test()
        {
            _instance.PostProcess();

            _processor.Received(1).PostProcess(_globalContext);
        }

        [TestMethod]
        public void AssembleDocument_Test()
        {
            _globalContext.Css.Returns("body { background: blue; }");
            _globalContext.Js.Returns("window.alert('hello')");
            _globalContext.Html.Returns(@"<div>some additional tag</div>");
            var expectedOutput =
                @"<!DOCTYPE html><html><head><style>" +
                _globalContext.Css +
                @"</style></head><body>" +
                "<p>1</p><p>2</p>" +
                _globalContext.Html +
                @"<script>" +
                _globalContext.Js +
                @"</script></body></html>";

            _instance.AssembleDocument();

            _stream.Position = 0;
            string result = new StreamReader(_stream).ReadToEnd();

            Assert.AreEqual(expectedOutput, result);
        }

        private static string BuildExpectedOutput(string[] elementsOutput)
        {
            return String.Join("\r\n", elementsOutput) + "\r\n";
        }

        private void AssertHasProcessedAllRootElements()
        {
            foreach (var elem in _globalContext.RootElements)
            {
                IElementContext processorContextArg = BuildProcessArgValidator(elem.RootElement);
                _processor.Received(1).ProcessElement(processorContextArg, Arg.Is(elem.RootElement));
            }
        }

        private IElementContext BuildProcessArgValidator(OpenXmlElement elem) => 
            Arg.Is<IElementContext>(c => c.RootElement == elem);

    }
}
