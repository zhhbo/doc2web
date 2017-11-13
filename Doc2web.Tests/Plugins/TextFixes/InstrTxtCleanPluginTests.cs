using Doc2web.Plugins.TextFixes;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Tests.Plugins.TextFixes
{
    [TestClass]
    public class InstrTxtCleanPluginTests
    {
        private InstrTxtCleanupPlugin _instance;
        private IElementContext _context;
        private List<Mutation> _mutations;

        [TestInitialize]
        public void Initialize()
        {
            _instance = new InstrTxtCleanupPlugin();
            _mutations = new List<Mutation>();
            _context = Substitute.For<IElementContext>();
            _context
                .When(x => x.AddMutation(Arg.Any<Mutation>()))
                .Do(x => _mutations.Add(x.ArgAt<Mutation>(0)));
        }

        [TestMethod]
        public void ProcessElement_Test()
        {
            var p = new Paragraph(
                new Run(new Text("1234")),
                new Run(new FieldCode("to-be-removed")),
                new Run(new Text("5678"))
            );

            _instance.ProcessElement(_context, p);

            Assert.AreEqual(1, _mutations.Count);
            var deletion = _mutations[0] as TextDeletion;
            Assert.IsNotNull(deletion);
            Assert.AreEqual(4.0, deletion.Position);
            Assert.AreEqual(Convert.ToDouble("to-be-removed".Length), deletion.Offset);
        }

    }
}
