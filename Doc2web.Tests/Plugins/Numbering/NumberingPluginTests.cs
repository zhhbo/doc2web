using Autofac;
using Doc2web.Plugins.Numbering;
using Doc2web.Plugins.Numbering.Mapping;
using Doc2web.Tests.Samples;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Doc2web.Tests.Plugins.Numbering
{
    [TestClass]
    public class NumberingPluginTests
    {
        private NumberingPlugin _instance;
        private NumberingPluginConfig _config;
        private WordprocessingDocument _wpDoc;

        [TestInitialize]
        public void Initialize()
        {
            MockDocument();
            _config = new NumberingPluginConfig();
            _instance = new NumberingPlugin(_wpDoc, _config);
        }

        private void MockDocument()
        {
            _wpDoc = WordprocessingDocument.Create(
                new MemoryStream(),
                DocumentFormat.OpenXml.WordprocessingDocumentType.Document);
            _wpDoc.AddMainDocumentPart();
            _wpDoc.MainDocumentPart.AddNewPart<NumberingDefinitionsPart>();
            _wpDoc.MainDocumentPart.AddNewPart<StyleDefinitionsPart>();
            _wpDoc.MainDocumentPart.Document =
                new DocumentFormat.OpenXml.Wordprocessing.Document(
                    DocumentSample1.GenerateBody());
            _wpDoc.MainDocumentPart.NumberingDefinitionsPart.Numbering =
                    DocumentSample1.GenerateNumbering();
            _wpDoc.MainDocumentPart.StyleDefinitionsPart.Styles =
                DocumentSample1.GenerateStyles();
        }

        [TestMethod]
        public void InitEngine_Test()
        {
            var containerBuilder = new ContainerBuilder();

            _instance.InitEngine(containerBuilder);
            var container = containerBuilder.Build();

            Assert.IsNotNull(container.Resolve<INumberingMapper>());
            Assert.AreEqual(_config, container.Resolve<NumberingPluginConfig>());
        }
    }
}
