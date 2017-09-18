using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Core.Rendering
{
    public class OpeningTag : SelfClosingTag
    {
        public OpeningTag() : base() { }

        public ClosingTag Related { get; set; }

        public string TextAfter { get; set; }

        public override double RelatedIndex => Related.Index;

        public override string Render() =>
            (Attributes.Keys.Any()) ?
            $"<{Name} {RenderAttributes()}>{TextAfter}" :
            $"<{Name}>{TextAfter}";
    }
}
