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
        public void InitEngine_Test()
        {
            MockDocument();
            Initialize();
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterInstance(new StyleConfig());

            _instance.InitEngine(containerBuilder);
            var container = containerBuilder.Build();

            Assert.IsNotNull(container.Resolve<INumberingMapper>());
            Assert.AreEqual(_config, container.Resolve<NumberingConfig>());
            Assert.IsTrue(container.TryResolve(
                typeof(ICssProperty),
                out object t));
            Assert.IsInstanceOfType(t, typeof(NumberingIndentationCssProperty));
        }

        [TestMethod]
        public void InsertNumbering_NullNumberingTest()
        {
            MockElementContext(1, 2, "1.1.1");
            _nMapper.IsValid.Returns(false);

            _instance.InsertNumbering(_elementContext, _p);


            Assert.AreEqual(0, _nodes.Count);
        }

        [TestMethod]
        public void InsertNumbering_ContainerMaxTest()
        {
            MockElementContext(1, 2, "1.1.1");

            _instance.InsertNumbering(_elementContext, _p);

            var containerMax = _nodes[0];
            AssertCountNodesBefore(0, containerMax);
            AssertCountNodesAfter(0, containerMax);
            AssertHasZAndTag(containerMax);
            AssertHasClasses(
                containerMax,
                "numbering-1-2",
                _config.NumberingContainerMaxCls);
            _cssRegistrator.Received(1).RegisterNumbering(1, 2);
        }


        [TestMethod]
        public void InsertNumbering_ContainerMinTest()
        {
            MockElementContext(1, 2, "1.1.1");

            _instance.InsertNumbering(_elementContext, _p);

            var containerMin = _nodes[1];
            AssertCountNodesBefore(1, containerMin);
            AssertCountNodesAfter(1, containerMin);
            AssertHasZAndTag(containerMin);
            AssertHasClasses(containerMin, _config.NumberingContainerMinCls);
        }

        [TestMethod]
        public void InsertNumbering_NumberMaxTest()
        {
            MockElementContext(1, 2, "1.1.1");

            _instance.InsertNumbering(_elementContext, _p);

            var numberMax = _nodes[2];
            AssertCountNodesBefore(2, numberMax);
            AssertCountNodesAfter(2, numberMax);
            AssertHasZAndTag(numberMax);
            AssertHasClasses(numberMax, _config.NumberingNumberMaxCls);
        }

        [TestMethod]
        public void InsertNumbering_NumberMinTest()
        {
            MockElementContext(1, 2, "1.1.1");

            _instance.InsertNumbering(_elementContext, _p);

            var numberMin = _nodes[3];
            AssertCountNodesBefore(3, numberMin);
            AssertCountNodesAfter(3, numberMin);
            Assert.AreEqual(_config.NumberingNumberTag, numberMin.Tag);
            Assert.AreEqual(_config.NumberingNumberZ, numberMin.Z);
            AssertHasClasses(numberMin, _config.NumberingNumberMinCls);
            Assert.AreEqual("1.5em", numberMin.Style["padding-right"]);
        }

        [TestMethod]
        public void InsertNumbering_NumberingMinDynTest()
        {
            MockElementContext(1, 2, "1.1.1");
            string clsName = MockDynamic();

            _instance.InsertNumbering(_elementContext, _p);

            var numberMin = _nodes[3];
            AssertHasClasses(
                numberMin,
                _config.NumberingNumberMinCls,
                clsName);
        }

        [TestMethod]
        public void InsertNumbering_NubmeringMinPaddingSpaceTest()
        {
            MockElementContext(1, 2, "1.1.1");
            _level.LevelSuffix = new LevelSuffix { Val = LevelSuffixValues.Space };

            _instance.InsertNumbering(_elementContext, _p);

            var numberMin = _nodes[3];
            Assert.AreEqual("0.5em", numberMin.Style["padding-right"]);
        }

        [TestMethod]
        public void InsertNumbering_NubmeringMinNoPaddingTest()
        {
            MockElementContext(1, 2, "1.1.1");
            _level.LevelSuffix = new LevelSuffix { Val = LevelSuffixValues.Nothing };

            _instance.InsertNumbering(_elementContext, _p);

            var numberMin = _nodes[3];
            Assert.IsFalse(numberMin.Style.ContainsKey("padding-right"));
        }

        [TestMethod]
        public void InsertNumbering_MutationTest()
        {
            MockElementContext(1, 2, "1.1.1");

            _instance.InsertNumbering(_elementContext, _p);

            var mutation = _mutations.Single();
            Assert.IsFalse(_nodes.Any(x => x.Start > mutation.Position));
            Assert.IsFalse(_nodes.Any(x => x.End < mutation.Position));

            var textInsert = mutation as TextInsertion;
            Assert.IsNotNull(textInsert);
            Assert.AreEqual("1.1.1", textInsert.Text);
        }


        private void AssertHasZAndTag(HtmlNode node)
        {
            Assert.AreEqual(_config.NumberingContainerTag, node.Tag);
            Assert.AreEqual(_config.NumberingContainerZ, node.Z);
        }

        private void AssertCountNodesBefore(int count, HtmlNode node)
        {
            Assert.AreEqual(count, _nodes.Where(x => x.Start < node.Start).Count());
        }

        private void AssertCountNodesAfter(int count, HtmlNode node)
        {
            Assert.AreEqual(count, _nodes.Where(x => x.End > node.End).Count());
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
                .RegisterNumbering(numberingId, levelId)
                .Returns($"numbering-{numberingId}-{levelId}");

            _elementContext = Substitute.For<IElementContext>();
            _elementContext.Resolve<INumberingMapper>().Returns(_nMapper);
            _elementContext.Resolve<ICssRegistrator>().Returns(_cssRegistrator);
            _elementContext.Element.Returns(_p);
            _elementContext.RootElement.Returns(_p);
            _elementContext
                .When(x => x.AddNode(Arg.Any<HtmlNode>()))
                .Do(x => _nodes.Add(x.ArgAt<HtmlNode>(0)));
            _elementContext
                .When(x => x.AddMutation(Arg.Any<Mutation>()))
                .Do(x => _mutations.Add(x.ArgAt<Mutation>(0)));
        }

        private string MockDynamic()
        {
            string clsName = $"dyn-{Guid.NewGuid().ToString().Replace("-", "")}";
            var pMarkRunProps = new ParagraphMarkRunProperties();
            _p.ParagraphProperties = new ParagraphProperties(pMarkRunProps);
            _cssRegistrator
                .RegisterRunProperties(Arg.Is(pMarkRunProps))
                .Returns(clsName);
            return clsName;
        }

        [TestMethod]
        public void PostProcessing_Test()
        {
            var context = Substitute.For<IGlobalContext>();
            context.Resolve<StyleConfig>().Returns(new StyleConfig());
            context.Resolve<NumberingConfig>().Returns(new NumberingConfig());
            string expectedCss =
                ".leftspacer, .numbering-container-min { display: flex; } " +
                ".numbering-number-min { white-space: pre; }";

            _instance.PostProcessing(context);

            context.Received(1).AddCss(expectedCss);
        }
    }
}
