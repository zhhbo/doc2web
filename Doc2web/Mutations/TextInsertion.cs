using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web
{
    public class TextInsertion : Mutation
    {
        public int Index { get; set; }

        public string Text { get; set; }

        public override void MutateNodes(List<HtmlNode> nodes)
        {
            foreach(var node in nodes)
            {
                if (node.Start > Index)
                    node.Start += Text.Length;
                if (node.End > Index)
                    node.End += Text.Length;
            }
        }

        public override void MutateText(StringBuilder stringBuilder)
        {
            stringBuilder.Insert(Index, Text);
        }
    }
}
