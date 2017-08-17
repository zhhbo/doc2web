using DocumentFormat.OpenXml;
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
    public class ProcessorTests
    {
        private List<string> _calledActions;
        private Processor _instance;

        [TestInitialize]
        public void Initialize()
        {
            _calledActions = new List<string>();
            _instance = new Processor();
        }

        [TestMethod]
        public void Processor_Test()
        {
            Assert.IsNotNull(_instance.PreRenderingActions);
            Assert.IsNotNull(_instance.PostRenderingActions);
            Assert.IsNotNull(_instance.ElementRenderingActions);
        }

        [TestMethod]
        public void Processor_CombinationTest()
        {
            Processor instanceA = BuildProcessorA();
            Processor instanceB = BuildProcessorB();

            _instance = new Processor(instanceA, instanceB);

            AssertContainsAllProcesssingActions(instanceA);
            AssertContainsAllProcesssingActions(instanceB);
        }

        [TestMethod]
        public void PreProcess_Test()
        {
            AddPreRenderingAction("A");
            AddPreRenderingAction("B");
            AddPreRenderingAction("C");
            AddPreRenderingAction("D");

            _instance.PreRender(Substitute.For<IGlobalContext>());

            Assert.AreEqual(4, _calledActions.Count);
            _calledActions.Contains("A");
            _calledActions.Contains("B");
            _calledActions.Contains("C");
            _calledActions.Contains("D");
        }


        [TestMethod]
        public void PostProcess_Tests()
        {
            AddPostRenderingAction("A");
            AddPostRenderingAction("B");
            AddPostRenderingAction("C");
            AddPostRenderingAction("D");

            _instance.PostRender(Substitute.For<IGlobalContext>());

            Assert.AreEqual(4, _calledActions.Count);
            _calledActions.Contains("A");
            _calledActions.Contains("B");
            _calledActions.Contains("C");
            _calledActions.Contains("D");
        }


        [TestMethod]
        public void ElementProcess_Test()
        {
            var context = Substitute.For<IElementContext>();
            AddElementRenderingAction<Paragraph>("A1");
            AddElementRenderingAction<Paragraph>("A2");
            AddElementRenderingAction<Run>("B");

            _instance.ElementRender(context, new Paragraph());

            Assert.AreEqual(2, _calledActions.Count);
            _calledActions.Sort();
            Assert.AreEqual("A1", _calledActions[0]);
            Assert.AreEqual("A2", _calledActions[1]);
        }

        private static Processor BuildProcessorA()
        {
            var processor = new Processor();
            processor.PreRenderingActions.Add(c => { });
            processor.ElementRenderingActions
                .Add(typeof(Paragraph), new List<Action<IElementContext, OpenXmlElement>>()
            {
                (c, e) => { },
                (c, e) => { },
                (c, e) => { },
            });
            return processor;
        }

        private static Processor BuildProcessorB()
        {
            var processor = new Processor();
            processor.PostRenderingActions.Add(c => { });
            processor.ElementRenderingActions
                .Add(typeof(Paragraph), new List<Action<IElementContext, OpenXmlElement>>()
            {
                (c, e) => { },
                (c, e) => { },
                (c, e) => { },
            });
            processor.ElementRenderingActions
                .Add(typeof(Run), new List<Action<IElementContext, OpenXmlElement>>()
            {
                (c, e) => { },
                (c, e) => { },
                (c, e) => { },
            });
            return processor;
        }


        private void AssertContainsAllProcesssingActions(Processor child)
        {
            AssertContainsAllPreProcessingActions(child);
            AssertContainsAllPostProcessingActions(child);
            AssertContainsAllElementProcessingActions(child);
        }

        private void AssertContainsAllElementProcessingActions(Processor child)
        {
            foreach (var key in child.ElementRenderingActions.Keys)
            {
                Assert.IsTrue(_instance.ElementRenderingActions.ContainsKey(key));

                var parentActions = _instance.ElementRenderingActions[key];
                foreach (var childAction in child.ElementRenderingActions[key])
                    Assert.IsTrue(parentActions.Contains(childAction));
            }
        }

        private void AssertContainsAllPostProcessingActions(Processor child)
        {
            foreach (var postProcessorAction in child.PostRenderingActions)
                Assert.IsTrue(_instance.PostRenderingActions.Contains(postProcessorAction));
        }

        private void AssertContainsAllPreProcessingActions(Processor child)
        {
            foreach (var preProcessorAction in child.PreRenderingActions)
                Assert.IsTrue(_instance.PreRenderingActions.Contains(preProcessorAction));
        }

        private void AddPreRenderingAction(string key)
        {
            _instance.PreRenderingActions.Add(BuildPrePostProcessingAction(key));
        }

        private void AddPostRenderingAction(string v)
        {
            _instance.PostRenderingActions.Add(BuildPrePostProcessingAction(v));
        }

        private void AddElementRenderingAction<T>(string key)
            where T : OpenXmlElement
        {
            var (type, action) = BuildElementProcessingAction<T>(_calledActions, key);
            var elementRendering = _instance.ElementRenderingActions;
            if (elementRendering.ContainsKey(type))
                elementRendering[type].Add(action);
            else
            {
                var list = new List<Action<IElementContext, OpenXmlElement>>();
                list.Add(action);
                elementRendering.Add(type, list);
            }

        }

        private Action<IGlobalContext> BuildPrePostProcessingAction(string key) =>
            (IGlobalContext _) => _calledActions.Add(key);

        private (Type, Action<IElementContext, OpenXmlElement>) BuildElementProcessingAction<T>(List<string> whenCalled, string key)
            where T : OpenXmlElement =>
            (typeof(T), (IElementContext cts, OpenXmlElement e) =>
            {
                if (e is T == false)
                    throw new Exception("The element does not match the kind targeted by the processor");
                whenCalled.Add(key);
            }
        );
    }
}


