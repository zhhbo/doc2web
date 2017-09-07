using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web
{
    public class TextDeletion : Mutation
    {

        public int Length { get; set; }

        private int End => Index + Length;

        public override void UpdateOtherMutations(List<Mutation> mutations)
        {
            int i = 0;
            while (i < mutations.Count)
            {
                var m = mutations[i];
                if (m.Index >= Index)
                {
                    m.Index -= Length;
                    if (m.Index < 0)
                    {
                        mutations.RemoveAt(i);
                        continue;
                    }
                }
                i++;
            }
        }

        public override void MutateText(StringBuilder text)
        {
            text.Remove(Index, Length);
        }

        public override void MutateNodes(List<HtmlNode> nodes)
        {
            _nodes = nodes;
            currentIndex = 0;
            while (currentIndex < nodes.Count)
                FixCurrent();
        }

        private void FixCurrent()
        {
            if (IsSurrounded) {
                RemoveCurrentNode();
                return;
            }
            else if (CurrentHasColisionStart) FixColisitonStart();
            else if (CurrentHasColisionEnd) FixColisitionEnd();
            else if (CurrentStartsAfter) FixStartsAfter();
            else if (CurrentEndsFater) FixEndsAfter();
            currentIndex += 1;
        }

        private List<HtmlNode> _nodes;

        private int currentIndex;

        private HtmlNode Current => _nodes[currentIndex];

        private bool IsSurrounded => 
            Current.Start >= Index && Current.End <= End;

        private bool CurrentHasColisionStart => 
            Current.Start < Index && Index < Current.End && Current.End < End;

        private bool CurrentHasColisionEnd => 
            Current.Start > Index && Current.Start < End && Current.End > End;

        private bool CurrentStartsAfter => Current.Start > Index;

        private bool CurrentEndsFater => Current.End > Index;

        private void RemoveCurrentNode()
        {
            _nodes.RemoveAt(currentIndex);
        }

        private void FixColisitonStart()
        {
            Current.End = Index;
        }

        private void FixColisitionEnd()
        {
            Current.Start = Index;
            Current.End = Index + (Current.End - End);
        }

        private void FixStartsAfter()
        {
            Current.Start -= Length;
            Current.End -= Length;
        }

        private void FixEndsAfter()
        {
            Current.End -= Length;
        }

    }
}
