using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web
{
    public abstract class Mutation
    {
        public abstract void MutateNodes(List<HtmlNode> nodes);
        public abstract void MutateText(StringBuilder text);
    }
}
