using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Core.Rendering
{
    public class OpeningTag : SelfClosingTag
    {
        public ClosingTag Related { get; set; }

        public override int RelatedIndex => Related.Index;

        public override string Render()
        {
            return base.Render();
        }
    }
}
