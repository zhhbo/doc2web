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

        [TestMethod]
        public void Processor_Test()
        {
            var instance = new Processor();

            Assert.IsNotNull(instance.PreRendering);
            Assert.IsNotNull(instance.PostRendering);
            Assert.IsNotNull(instance.ElementRendering);
        }

        [TestMethod]
        public void Processor_CombinationTest()
        {
            Processor instanceA = InitTestProcessorA();
            Processor instanceB = InitTestProcessorB();

            var instance = new Processor(instanceA, instanceB);

            AssertContainsAllActions(instance, instanceA);
        }

        private void AssertContainsAllActions(Processor parent, Processor child)
        {
            foreach (var preProcessorAction in child.PreRendering)
                Assert.IsTrue(parent.PreRendering.Contains(preProcessorAction));

            foreach (var postProcessorAction in child.PostRendering)
                Assert.IsTrue(parent.PostRendering.Contains(postProcessorAction));

            foreach (var key in child.ElementRendering.Keys)
            {
                Assert.IsTrue(parent.ElementRendering.ContainsKey(key));

                var parentActions = parent.ElementRendering[key];
                foreach (var childAction in child.ElementRendering[key])
                    Assert.IsTrue(parentActions.Contains(childAction));
            }
        }

        private static Processor InitTestProcessorA()
        {
            var instanceA = new Processor();
            instanceA.PreRendering.Add(c => { });
            instanceA.ElementRendering
                .Add(typeof(Paragraph), new List<Action<IElementContext, OpenXmlElement>>()
            {
                (c, e) => { },
                (c, e) => { },
                (c, e) => { },
            });
            return instanceA;
        }

        private static Processor InitTestProcessorB()
        {
            var instanceB = new Processor();
            instanceB.PostRendering.Add(c => { });
            instanceB.ElementRendering
                .Add(typeof(Paragraph), new List<Action<IElementContext, OpenXmlElement>>()
            {
                (c, e) => { },
                (c, e) => { },
                (c, e) => { },
            });
            instanceB.ElementRendering
                .Add(typeof(Run), new List<Action<IElementContext, OpenXmlElement>>()
            {
                (c, e) => { },
                (c, e) => { },
                (c, e) => { },
            });
            return instanceB;
        }

        [TestMethod]
        public void PreProcess_Test()
        {
            var calledActions = new List<string>();
            var instance = new Processor();
            instance.PreRendering.Add((BuildAction(calledActions, "A")));
            instance.PreRendering.Add((BuildAction(calledActions, "B")));
            instance.PreRendering.Add((BuildAction(calledActions, "C")));
            instance.PreRendering.Add((BuildAction(calledActions, "D")));

            instance.PreRender(Substitute.For<IGlobalContext>());

            Assert.AreEqual(4, calledActions.Count);
            calledActions.Contains("A");
            calledActions.Contains("B");
            calledActions.Contains("C");
            calledActions.Contains("D");
        }

        [TestMethod]
        public void PostProcess_Tests()
        {
            var calledActions = new List<string>();
            var instance = new Processor();
            instance.PostRendering.Add((BuildAction(calledActions, "A")));
            instance.PostRendering.Add((BuildAction(calledActions, "B")));
            instance.PostRendering.Add((BuildAction(calledActions, "C")));
            instance.PostRendering.Add((BuildAction(calledActions, "D")));

            instance.PostRender(Substitute.For<IGlobalContext>());

            Assert.AreEqual(4, calledActions.Count);
            calledActions.Contains("A");
            calledActions.Contains("B");
            calledActions.Contains("C");
            calledActions.Contains("D");
        }

        private Action<IGlobalContext> BuildAction(List<string> whenCalled, string key) =>
            (IGlobalContext _) => whenCalled.Add(key);

        [TestMethod]
        public void ElementProcess_Test()
        {
            var calledActions = new List<string>();
            var context = Substitute.For<IElementContext>();
            var instance = new Processor();
            AddElementRenderingAction<Paragraph>(instance, calledActions, "A1");
            AddElementRenderingAction<Paragraph>(instance, calledActions, "A2");
            AddElementRenderingAction<Run>(instance, calledActions, "B");

            instance.ElementRender(context, new Paragraph());

            Assert.AreEqual(2, calledActions.Count);
            calledActions.Sort();
            Assert.AreEqual("A1", calledActions[0]);
            Assert.AreEqual("A2", calledActions[1]);
        }


        private void AddElementRenderingAction<T>(Processor processor, List<string> whenCalled, string key)
            where T : OpenXmlElement
        {
            var (type, action) = BuildElementAction<T>(whenCalled, key);
            var elementRendering = processor.ElementRendering;
            if (elementRendering.ContainsKey(type))
                elementRendering[type].Add(action);
            else
            {
                var list = new List<Action<IElementContext, OpenXmlElement>>();
                list.Add(action);
                elementRendering.Add(type, list);
            }

        }

        private (Type, Action<IElementContext, OpenXmlElement>) BuildElementAction<T>(List<string> whenCalled, string key)
            where T : OpenXmlElement =>
            (typeof(T), (IElementContext cts, OpenXmlElement e) => {
                if (e is T == false)
                    throw new Exception("The element does not match the kind targeted by the processor");
                whenCalled.Add(key);
            });
    }
}


