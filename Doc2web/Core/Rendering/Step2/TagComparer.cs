using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Core.Rendering.Step2
{
    public class TagComparer : IComparer<ITag>
    {
        public int Compare(ITag x, ITag y)
        {
            if (x.Index < y.Index) return -1;
            if (y.Index < x.Index) return 1;
            return TypeByType(x, y);
        }

        private int TypeByType(ITag x, ITag y)
        {
            switch (x)
            {
                case OpeningTag xOpening: return CompareOpeningWith(xOpening, y); 
                case ClosingTag xClosing: return CompareClosingWith(xClosing, y);
                default: return CompareSelfClosingWith((SelfClosingTag)x, y);
            }
        }

        private int CompareOpeningWith(OpeningTag x, ITag y)
        {
            switch (y)
            {
                case OpeningTag yOpening: return CompareTwoOpening(x, yOpening); 
                case ClosingTag yClosing: return CompareOpeningClosing(x, yClosing);
                default: return CompareOpeningSelfClosing(x, (SelfClosingTag)y);
            }
        }

        private int CompareClosingWith(ClosingTag x, ITag y)
        {
            switch (y)
            {
                case OpeningTag yOpening: return CompareOpeningClosing(yOpening, x) * -1; 
                case ClosingTag yClosing: return CompareTwoClosing(x, yClosing);
                default: return CompareClosingSelfClosing(x, (SelfClosingTag)y);
            }
        }

        private int CompareSelfClosingWith(SelfClosingTag x, ITag y)
        {
            switch (y)
            {
                case OpeningTag yOpening: return CompareOpeningSelfClosing(yOpening, x) * -1;
                case ClosingTag yClosing: return CompareClosingSelfClosing(yClosing, x) * -1;
                default: return CompareTwoSelfClosing(x, (SelfClosingTag)y);
            }
        }

        private int CompareTwoOpening(OpeningTag x, OpeningTag y)
        {
            if (x.RelatedIndex > y.RelatedIndex) return -1;
            if (y.RelatedIndex > x.RelatedIndex) return 1;
            if (x.Z > y.Z) return -1;
            if (y.Z > x.Z) return 1;
            return x.Name.CompareTo(y.Name);
        }

        private int CompareTwoClosing(ClosingTag x, ClosingTag y)
        {
            if (x.RelatedIndex > y.RelatedIndex) return -1;
            if (y.RelatedIndex > x.RelatedIndex) return -1;
            if (x.Z < y.Z) return -1;
            if (y.Z < x.Z) return 1;
            return x.Name.CompareTo(y.Name) * -1;
        }

        private int CompareTwoSelfClosing(SelfClosingTag x, SelfClosingTag y)
        {
            if (x.Z > y.Z) return -1;
            if (y.Z > x.Z) return 1;
            return x.Name.CompareTo(y.Name);
        }

        private int CompareOpeningClosing(OpeningTag x, ClosingTag y)
        {
            return 1;
        }

        private int CompareOpeningSelfClosing(OpeningTag x, SelfClosingTag y)
        {
            if (x.Z > y.Z) return -1;
            if (y.Z > x.Z) return 1;
            return x.Name.CompareTo(y.Name);
        }

        private int CompareClosingSelfClosing(ClosingTag x, SelfClosingTag y)
        {
            if (x.Z > y.Z) return 1;
            if (y.Z > x.Z) return -1;
            return x.Name.CompareTo(y.Name);
        }
    }
}
