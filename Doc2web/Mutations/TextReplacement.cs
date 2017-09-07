using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web
{
    public class TextReplacement : Mutation
    {
        public int Length { get; set; }
        public string Replacement { get; set; }

        private class StubMutation : Mutation { }

        public override void UpdateOtherMutations(List<Mutation> mutations)
        {
            DecoratedMutation.UpdateOtherMutations(mutations);
        }

        public override void MutateNodes(List<HtmlNode> nodes)
        {
            DecoratedMutation.MutateNodes(nodes);
        }

        private Mutation DecoratedMutation
        {
            get
            {
                if (ReplacementIsBigger)
                    return new TextInsertion
                    {
                        Index = Index + Length,
                        Text = Replacement.Substring(Length)
                    };
                else if (ReplacementIsSmaller)
                    return new TextDeletion
                    {
                        Index = Index + Replacement.Length,
                        Length = Length - Replacement.Length
                    };
                return new StubMutation();
            }
        }

        public override void MutateText(StringBuilder text)
        {
            if (ReplacementIsBigger)
                ReplaceBigger(text);
            else if (ReplacementIsSmaller)
                ReplaceSmaller(text);
            else
                ReplaceSameSize(text);
        }

        private bool ReplacementIsBigger => Replacement.Length > Length;

        private bool ReplacementIsSmaller => Replacement.Length < Length;

        private void ReplaceBigger(StringBuilder text)
        {
            ReplaceSameSize(text);
            text.Insert(Index + Length, Replacement.Substring(Length));
        }

        private void ReplaceSmaller(StringBuilder text)
        {
            for (int i = 0; i < Replacement.Length; i++)
                text[Index + i] = Replacement[i];
            text.Remove(Index + Replacement.Length, Length - Replacement.Length);
        }

        private void ReplaceSameSize(StringBuilder text)
        {
            for (int i = 0; i < Length; i++)
                text[Index + i] = Replacement[i];
        }
    }
}
