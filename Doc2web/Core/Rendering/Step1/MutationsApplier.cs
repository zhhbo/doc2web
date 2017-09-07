using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Core.Rendering.Step1
{
    public class MutationsApplier 
    {
        private class MutationComparer : IComparer<Mutation>
        {
            public int Compare(Mutation x, Mutation y) => y.Index - x.Index;
        }

        public static void Apply(List<HtmlNode> nodes, List<Mutation> mutations)
        {
            mutations.Sort(new MutationComparer());

            for(int i =0; i < mutations.Count; i++)
            {
                var mutation = mutations[i];

                mutation.MutateNodes(nodes);

                UpdateFollowingMutations(mutations, i, mutation);
            }
        }

        private static void UpdateFollowingMutations(List<Mutation> mutations, int i, Mutation mutation)
        {
            if (i == mutations.Count - 1) return;
            i++;
            var nextMutations = mutations.GetRange(i, mutations.Count - i);
            mutation.UpdateOtherMutations(nextMutations);
            mutations.RemoveRange(i, mutations.Count - i);
            mutations.AddRange(nextMutations);
        }
    }
}
