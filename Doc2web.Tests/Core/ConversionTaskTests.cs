﻿using Autofac;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Doc2web.Core;

namespace Doc2web.Tests.Core
{
    [TestClass]
    public class ConversionTaskTests
    {
        private ConversionTask _instance;
        private IGlobalContext _globalContext;
        private IContainer _engineContainer;
        private OpenXmlElement[] _rootElements;
        private IProcessor _processor;
        private IContextRenderer _contextRenderer;

        [TestInitialize]
        public void Initialize()
        {
            _globalContext = Substitute.For<IGlobalContext>();
            _engineContainer = Substitute.For<IContainer>();
            _rootElements = new OpenXmlElement[] { };
            _processor = Substitute.For<IProcessor>();
            _contextRenderer = Substitute.For<IContextRenderer>();

            BuildInstance();
        }

        private void BuildInstance()
        {
            _instance = new ConversionTask
            {
                GlobalContext = _globalContext,
                Processor = _processor,
                ContextRenderer = _contextRenderer,
                Container = _engineContainer,
                RootElements = _rootElements
            };
        }

        [TestMethod]
        public void Initialize_Test()
        {
            _instance.GlobalContext = null;
            _instance.Initialize();

            Assert.IsNotNull(_instance.GlobalContext);
            _engineContainer.Received(1).BeginLifetimeScope(_processor.InitProcess);
        }

        [TestMethod]
        public void PreProcess_Test()
        {
            _instance.PreProcess();

            _processor.Received(1).PreProcess(_globalContext);
        }

        [TestMethod]
        public void ConvertElements_Test()
        {
            var elements = new OpenXmlElement[] { 
                new Paragraph(),
                new Paragraph()
            };
            _globalContext.RootElements.Returns(elements);

            _instance.ConvertElements();

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
                _globalContext.Html +
                @"<script>" +
                _globalContext.Js +
                @"</script></body></html>";

            _instance.AssembleDocument();
            var result = _instance.Result;

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
                IElementContext processorContextArg = BuildProcessArgValidator(elem);
                _processor.Received(1).ProcessElement(processorContextArg, Arg.Is(elem));
            }
        }

        private IElementContext BuildProcessArgValidator(OpenXmlElement elem) => 
            Arg.Is<IElementContext>(c => c.RootElement == elem);

    }
}
