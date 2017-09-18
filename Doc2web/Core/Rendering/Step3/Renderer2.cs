using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Core.Rendering.Step3
{
    public class Renderer2 : IComparer<IRenderable>
    {
        public string Text { get; set; }

        public IRenderable[] Elements { get; set; }

        private double top;

        private int LimText => Text.Length;

        public string Render()
        {
            StringBuilder sb = new StringBuilder();
            top = double.MinValue;
            Array.Sort(Elements, this);

            foreach(var e in Elements)
            {
                if ((top <= 0 && e.Position >= LimText) ||
                    (top >= 0 && LimText >= e.Position) ||
                    (top <= 0.0 && 0.0 <= e.Position) ||
                    (top <= LimText && LimText <= e.Position) &&
                    (e.Position - top > 0))
                {
                    int index = (int)Math.Max(0, top);
                    int length = (int)Math.Min(LimText, e.Position) - index;
                    if (length > 0)
                    {
                        string ss = Text.Substring(index, length);
                        sb.Append(ss);
                    }
                }

                sb.Append(e.Render());
                if (top < e.Position + e.Offset)
                    top = e.Position + e.Offset;
            }

            if (top > 0 && top < LimText)
            {
                sb.Append(Text.Substring((int)top));
            }

            return sb.ToString();
        }

        public int Compare(IRenderable x, IRenderable y)
        {
            if (x.Position < y.Position) return -1;
            else if (x.Position > y.Position) return 1;
            return 0;
        }
    }
}
