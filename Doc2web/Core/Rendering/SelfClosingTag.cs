using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Core.Rendering
{
    public class SelfClosingTag : ITag
    {
        public SelfClosingTag()
        {
            Index = 0;
            Z = int.MinValue;
            Name = "div";
            Attributes = new Dictionary<string, string>();
        }

        public int Index { get; set; }

        public int Z { get; set; }

        public string Name { get; set; }

        public Dictionary<string, string> Attributes { get; set; }

        public virtual int RelatedIndex => Index;

        public virtual string Render() => String.Format("<{0} {1}/>", Name, RenderAttributes());

        protected string RenderAttributes() => String.Concat(RenderEachAttributes()).TrimStart();

        private IEnumerable<string> RenderEachAttributes()
        {
            foreach(var attribute in Attributes)
                yield return $" {attribute.Key}=\"{attribute.Value}\"";
        }
    }
}
