using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Core.Rendering
{
    public class SelfClosingTag : ITag
    {
        public int Index { get; set; }

        public int Z { get; set; }

        public string Name { get; set; }

        public Dictionary<string, string> Attributes { get; set; }

        public virtual int RelatedIndex => Index;

        public virtual string Render()
        {
            throw new NotImplementedException();
        }
    }
}
