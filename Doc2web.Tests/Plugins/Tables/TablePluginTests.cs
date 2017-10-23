using Doc2web.Plugins.Tables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Linq;

namespace Doc2web.Tests.Plugins.Tables
{
    [TestClass]
    public class TablePluginTests
    {
        private TablePlugin _instance;
        private IElementContext _elementContext;
        private TableConfig _config;
        private List<HtmlNode> _nodes;
        private List<Mutation> _mutations;
        private IGlobalContext _globalContext;
        private string _css;

        [TestInitialize]
        public void Initialize()
        {
            _config = new TableConfig();
            _nodes = new List<HtmlNode>();
            _mutations = new List<Mutation>();
            _css = "";
            _instance = new TablePlugin(_config);
            _elementContext = Substitute.For<IElementContext>();
            _elementContext
                .When(x => x.AddNode(Arg.Any<HtmlNode>()))
                .Do(x => _nodes.Add(x.ArgAt<HtmlNode>(0)));
            _elementContext
                .When(x => x.AddMutation(Arg.Any<Mutation>()))
                .Do(x => _mutations.Add(x.Arg<Mutation>()));
            _globalContext = Substitute.For<IGlobalContext>();
            _globalContext
                .When(x => x.AddCss(Arg.Any<string>()))
                .Do(x => _css += x.ArgAt<string>(0));
        }

        [TestMethod]
        public void ProcessTable_Test()
        {
            string content = "Some content.";
            _instance.ProcessTable(_elementContext, CreateTable(content));

            AssertDeleteAllText(content);
            AssertContainsOnlyWarningNode();
        }

        [TestMethod]
        public void PostProcessing_Test()
        {
            _instance.PostProcessing(_globalContext);

            Assert.AreEqual(TablePlugin.CSS(_config.WarningCssClass), _css);
        }

        private void AssertContainsOnlyWarningNode()
        {
            var expected = new HtmlNode()
            {
                Start = 0,
                End = 0,
                TextPrefix = _config.WarningMessage
            };
            expected.AddClasses(_config.WarningCssClass);
            var node = _nodes.Single();
            Assert.AreEqual(expected, node);
        }

        private void AssertDeleteAllText(string content)
        {
            var mutation = _mutations.Single() as TextDeletion;
            Assert.IsNotNull(mutation);
            Assert.AreEqual(0, mutation.Position);
            Assert.AreEqual(content.Length, mutation.Offset);
        }

        private static Table CreateTable(string content) =>
            new Table(
                new TableRow(
                    new TableCell(
                        new Paragraph(
                            new Run(
                                new Text(content))))));
    }
}
