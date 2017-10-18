using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Core.Rendering
{
    public class ClosingTag : ITag
    {
        public OpeningTag Related { get; set; }

        public string Name => Related.Name;

        public double Offset => 0;

        public string TextBefore { get; set; }

        public double Position { get; set; }

        public double RelatedPosition => Related.Position;

        public int Z { get; set; }

        public string Render() => $"{TextBefore}</{Related.Name}>";

        public override string ToString() => Render();

    }

}
