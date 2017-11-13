using Autofac;
using Doc2web.Plugins.Numbering;
using Doc2web.Plugins.Numbering.Mapping;
using Doc2web.Plugins.Style;
using Doc2web.Tests.Samples;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Doc2web.Plugins.Style.Properties;
using Doc2web.Plugins.Style.Css;

namespace Doc2web.Tests.Plugins.Numbering
{
    [TestClass]
    public class NumberingPluginTests
    {
        private NumberingPlugin _instance;
        private NumberingConfig _config;
        private WordprocessingDocument _wpDoc;
        private Paragraph _p;
        private INumberingMapper _nMapper;
        private IParagraphData _pData;
        private ICssRegistrator _cssRegistrator;
        private IElementContext _elementContext;
        private Level _level;
        private List<HtmlNode> _nodes;
        private List<Mutation> _mutations;

        [TestInitialize]
        public void Initialize()
        {
            _config = new NumberingConfig();
            _instance = new NumberingPlugin(_wpDoc, _config);
            _level = new Level();
            _nodes = new List<HtmlNode>();
            _mutations = new List<Mutation>();
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
                new Document(
                    DocumentSample1.GenerateBody());
            _wpDoc.MainDocumentPart.NumberingDefinitionsPart.Numbering =
                    DocumentSample1.GenerateNumbering();
            _wpDoc.MainDocumentPart.StyleDefinitionsPart.Styles =
                DocumentSample1.GenerateStyles();
        }

        [TestMethod]
        public void RegisterConfig_WordprocessingDocumentTest()
        {
            MockDocument();
            Initialize();
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterInstance(new StyleConfig());

            _instance.InitEngine(containerBuilder);
            var container = containerBuilder.Build();

            Assert.IsNotNull(container.Resolve<INumberingMapper>());
            Assert.AreEqual(_config, container.Resolve<NumberingConfig>());
        }

        [TestMethod]
        public void InitEngine_NumberingMapperTest()
        {
            var numberingMapper = Substitute.For<INumberingMapper>();
            _instance = new NumberingPlugin(numberingMapper, _config);
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterInstance(new StyleConfig());

            _instance.InitEngine(containerBuilder);
            var container = containerBuilder.Build();

            Assert.AreSame(numberingMapper, container.Resolve<INumberingMapper>());
            Assert.AreEqual(_config, container.Resolve<NumberingConfig>());
        }

        [TestMethod]
        public void InsertNumbering_InvalidNumberingTest()
        {
            MockElementContext(1, 2, "1.1.1");
            _nMapper.IsValid.Returns(false);

            _instance.InsertNumbering(_elementContext, _p);

            Assert.AreEqual(0, _nodes.Count);
        }

        [TestMethod]
        public void InsertNumbering_ContainerTest()
        {
            MockElementContext(1, 2, "1.1.1");

            _instance.InsertNumbering(_elementContext, _p);

            var numberingContainer = _nodes[0];
            Assert.AreEqual(_config.NumberingContainerTag, numberingContainer.Tag);
            Assert.AreEqual(_config.NumberingContainerZ, numberingContainer.Z);
            AssertHasClasses(numberingContainer, "leftspacer");
        }



        [TestMethod]
        public void InsertNumbering_NumberTest()
        {
            MockElementContext(1, 2, "1.1.1");

            _instance.InsertNumbering(_elementContext, _p);

            var numberingNumber = _nodes[1];
            Assert.AreEqual("1.1.1", numberingNumber.TextPrefix);
            Assert.AreEqual(_config.NumberingNumberTag, numberingNumber.Tag);
            Assert.AreEqual(_config.NumberingNumberZ, numberingNumber.Z);
            AssertHasClasses(numberingNumber, "dynamic-cls", _config.NumberingNumberCls);
            Assert.AreEqual("1.5em", numberingNumber.Style["padding-right"]);
        }

        [TestMethod]
        public void InsertNumbering_NumberNoPaddingTest()
        {
            MockElementContext(1, 2, "1.1.1");
            _level.LevelSuffix = new LevelSuffix { Val = LevelSuffixValues.Nothing };

            _instance.InsertNumbering(_elementContext, _p);

            var numberMin = _nodes[1];
            Assert.IsFalse(numberMin.Style.ContainsKey("padding-right"));
        }

        private void AssertHasClasses(HtmlNode node, params string[] classes)
        {
            Assert.AreEqual(classes.Length, node.Classes.Count);
            foreach (var c in classes)
                Assert.IsTrue(node.Classes.Contains(c));
        }

        private void MockElementContext(int numberingId, int levelId, string verbose)
        {
            _p = new Paragraph();
            _pData = Substitute.For<IParagraphData>();
            _pData.NumberingId.Returns(numberingId);
            _pData.LevelIndex.Returns(levelId);
            _pData.Verbose.Returns(verbose);
            _pData.LevelXmlElement.Returns(_level);
            _nMapper = Substitute.For<INumberingMapper>();
            _nMapper.IsValid.Returns(true);
            _nMapper.GetNumbering(_p).Returns(_pData);
            _cssRegistrator = Substitute.For<ICssRegistrator>();
            _cssRegistrator
                .RegisterRun(Arg.Any<ParagraphProperties>(), Arg.Any<RunProperties>(), (numberingId, levelId))
                .Returns(new CssClass { Name = $"dynamic-cls" });

            _elementContext = Substitute.For<IElementContext>();
            _elementContext.Resolve<INumberingMapper>().Returns(_nMapper);
            _elementContext.TryResolve(out ICssRegistrator z).Returns(x =>
            {
                x[0] = _cssRegistrator;
                return true;
            });
            _elementContext.Element.Returns(_p);
            _elementContext.RootElement.Returns(_p);
            _elementContext
                .When(x => x.AddNode(Arg.Any<HtmlNode>()))
                .Do(x => _nodes.Add(x.ArgAt<HtmlNode>(0)));
            _elementContext
                .When(x => x.AddMutation(Arg.Any<Mutation>()))
                .Do(x => _mutations.Add(x.ArgAt<Mutation>(0)));
        }

        [TestMethod]
        public void PostProcessing_Test()
        {
            var context = Substitute.For<IGlobalContext>();
            context.Resolve<StyleConfig>().Returns(new StyleConfig());
            context.Resolve<NumberingConfig>().Returns(new NumberingConfig());
            string expectedCss =
                ".leftspacer { display: block; text-align: right; } " +
                ".numbering-number { display: inline-block; white-space: pre; }";

            _instance.PostProcessing(context);

            context.Received(1).AddCss(expectedCss);
        }
    }
}
