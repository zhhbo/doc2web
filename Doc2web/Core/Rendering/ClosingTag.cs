using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Core.Rendering
{
    public class ClosingTag : ITag
    {
        public OpeningTag Related { get; set; }

        public string Name => Related.Name;

        public string TextBefore { get; set; }

        public double Index { get; set; }

        public double RelatedIndex => Related.Index;

        public double Z => Related.Z;

        public string Render() => $"{TextBefore}</{Related.Name}>";

    }

}
