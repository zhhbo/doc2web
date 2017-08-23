using Doc2web.Core.Rendering.Step1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Core.Rendering
{
    public class ContextRenderer : IContextRenderer
    {
        public string Render(IElementContext elementContext)
        {
            var nodes = FlatternHtmlNodes.Flattern(elementContext.Nodes.ToList());
            var tags = TagsFactory.Build(nodes);
            var result = new StringBuilder(elementContext.RootElementText);
            TagsRenderer.RenderInto(tags, result);
            return result.ToString();
        }
    }
}
