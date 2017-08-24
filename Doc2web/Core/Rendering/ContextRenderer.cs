using Doc2web.Core.Rendering.Step1;
using Doc2web.Core.Rendering.Step2;
using Doc2web.Core.Rendering.Step3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Core.Rendering
{
    public class ContextRenderer : IContextRenderer
    {
        public string Render(Doc2web.IElementContext elementContext)
        {
            var nodes = FlatternHtmlNodes.Flattern(elementContext.Nodes.ToList());
            var tags = TagsFactory.Build(nodes);
            var result = new StringBuilder(elementContext.RootElement.InnerText);
            TagsRenderer.RenderInto(tags, result);
            return result.ToString();
        }
    }
}
