using Doc2web.Core.Rendering.Step1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Tests.Core.Rendering
{
    [TestClass]
    public class MutationsApplierTests
    {
        private List<Mutation> mutations;

        private Mutation MockMutation(int i)
        {
            var m = Substitute.For<Mutation>();
            m.Index = i;
            m.ClearReceivedCalls();
            return m;
        }

        [TestMethod]
        public void ApplyMutations_Test()
        {
            mutations = new List<Mutation>
            {
                MockMutation(0),
                MockMutation(3),
                MockMutation(6),
                MockMutation(9),
            };
            var nodes = new List<HtmlNode>() { }; 

            MutationsApplier.Apply(nodes, mutations);

            foreach(var m in mutations)
                m.Received(1).MutateNodes(nodes);

            AssertUpdatedMutationRange(mutations[0], 1, 3);
            AssertUpdatedMutationRange(mutations[1], 2, 2);
            AssertUpdatedMutationRange(mutations[2], 3, 1);
            mutations[3].DidNotReceive().UpdateOtherMutations(Arg.Any<List<Mutation>>());
        }

        private void AssertUpdatedMutationRange(Mutation m, int i, int l)
        {
            m.Received(1).UpdateOtherMutations(Arg.Any<List<Mutation>>());

            var arg = m.ReceivedCalls()
                .Single(x => x.GetMethodInfo().Name == "UpdateOtherMutations")
                .GetArguments()[0] as List<Mutation>;

            Assert.AreEqual(l, arg.Count);
            for (int j = 0; j < l; j++)
                Assert.AreSame(mutations[i + j], arg[j]);
        }
    }
}
