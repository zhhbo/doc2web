using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Core.Rendering
{
    public class SelfClosingTag : ITag
    {
        public SelfClosingTag()
        {
            Position = 0;
            Name = "div";
            Attributes = new Dictionary<string, string>();
        }

        public double Offset => 0;

        public double Position { get; set; }

        public string Name { get; set; }

        public IReadOnlyDictionary<string, string> Attributes { get; set; }

        public virtual double RelatedPosition => Position;

        public virtual string Render() => String.Format("<{0} {1}/>", Name, RenderAttributes());

        protected string RenderAttributes() => String.Concat(RenderEachAttributes()).TrimStart();

        private IEnumerable<string> RenderEachAttributes()
        {
            foreach(var attribute in Attributes)
                yield return $" {attribute.Key}=\"{attribute.Value}\"";
        }
    }
}
