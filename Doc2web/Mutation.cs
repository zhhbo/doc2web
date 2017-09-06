using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web
{
    public abstract class Mutation
    {
        public virtual void MutateNodes(List<HtmlNode> nodes) { }

        public virtual void MutateText(StringBuilder text) { }
    }
}
