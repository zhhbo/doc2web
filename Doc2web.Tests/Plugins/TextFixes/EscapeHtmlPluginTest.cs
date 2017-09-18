using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using NSubstitute;
using Doc2web.Plugins.TextFixes;
using System.Linq;

namespace Doc2web.Tests.Plugins.TextFixes
{
    [TestClass]
    public class EscapeHtmlPluginTest
    {
        private EscapeHtmlPlugin _instance;
        private List<Mutation> _mutations;
        private IElementContext _context;
        private string _text;

        [TestInitialize]
        public void Initialize()
        {
            _instance = new EscapeHtmlPlugin();
            _mutations = new List<Mutation>();
            _context = Substitute.For<IElementContext>();
            _context.TextIndex.Returns(0);
            _context
                .When(x => x.AddMutations(Arg.Any<IEnumerable<Mutation>>()))
                .Do(x => _mutations.AddRange(x.ArgAt<IEnumerable<Mutation>>(0)));
        }

        [TestMethod]
        public void EscapeCharacters_AmpTest()
        {
            _text = @"This is the & character.";
            var p = new Paragraph(new Run(new Text(_text)));

            _instance.EscapeCharacters(_context, p);

            AssertCharacterEscaped('&', "&amp;");
        }

        [TestMethod]
        public void EscapeCharacters_LtTest()
        {
            _text = @"This is the < character.";
            var p = new Paragraph(new Run(new Text(_text)));

            _instance.EscapeCharacters(_context, p);

            AssertCharacterEscaped('<', "&lt;");
        }

        [TestMethod]
        public void EscapeCharacters_GtTest()
        {
            _text = @"This is the > character.";
            var p = new Paragraph(new Run(new Text(_text)));

            _instance.EscapeCharacters(_context, p);

            AssertCharacterEscaped('>', "&gt;");
        }

        private void AssertCharacterEscaped(char charToEscape, string replacement)
        {
            var mutation = _mutations.Single() as TextReplacement;
            Assert.IsNotNull(mutation);
            Assert.AreEqual(_text.IndexOf(charToEscape), mutation.Position);
            Assert.AreEqual(1, mutation.Length);
            Assert.AreEqual(replacement, mutation.Replacement);
        }
    }
}
