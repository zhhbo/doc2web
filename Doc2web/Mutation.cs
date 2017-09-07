using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web
{
    public abstract class Mutation
    {
        public int Index { get; set; }

        public virtual void UpdateOtherMutations(List<Mutation> mutations) { }

        public virtual void MutateNodes(List<HtmlNode> nodes) { }

        public virtual void MutateText(StringBuilder text) { }
    }
}
