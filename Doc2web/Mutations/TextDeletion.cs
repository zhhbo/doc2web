using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web
{
    public class TextDeletion : Mutation
    {
        public int Index { get; set; }

        public int Length { get; set; }


        public override void MutateNodes(List<HtmlNode> nodes)
        {
            int end = Index + Length;
            int i = 0;
            while (i < nodes.Count)
            {
                var n = nodes[i];

                if (n.Start >= Index && n.End <= end)
                {
                    nodes.RemoveAt(i);
                    continue;
                } else if (n.Start < Index && Index < n.End && n.End < end)
                {
                    n.End = Index;
                } else if (n.Start > Index && n.Start < end && n.End > end)
                {
                    n.Start = Index;
                    n.End = Index + (n.End - end);

                } else if (n.Start > Index) {
                    n.Start -= Length;
                    n.End -= Length;
                } else if (n.End > Index)
                {
                    n.End -= Length;
                }

                i++;
            }
        }

        public override void MutateText(StringBuilder text)
        {
            text.Remove(Index, Length);
        }
    }
}
