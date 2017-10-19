using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Core.Rendering
{
    public class Stringifier
    {
        public string Text { get; set; }

        public IRenderable[] Elements { get; set; }

        private double top;

        private int LimText => Text.Length;

        public string Stringify()
        {
            StringBuilder sb = new StringBuilder();
            top = double.MinValue;
            Elements = Elements.OrderBy(x => x.Position).ToArray();

            foreach (var e in Elements)
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
    }
}
