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
        public string Render(IElementContext elementContext)
        {
            List<HtmlNode> nodes = BuildNodes(elementContext);
            ITag[] tags = BuildTags(nodes);
            return Render(elementContext, tags);
        }

        public static List<HtmlNode> BuildNodes(IElementContext elementContext)
        {
            var nodes = elementContext.Nodes.ToList();

            foreach (var m in elementContext.Mutations)
                m.MutateNodes(nodes);

            return FlatternHtmlNodes.Flattern(nodes);
        }

        public static ITag[] BuildTags(List<HtmlNode> nodes)
        {
            return TagsFactory.Build(nodes);
        }

        public static string Render(IElementContext elementContext, ITag[] tags)
        {
            var html = new StringBuilder(elementContext.RootElement.InnerText);

            foreach (var m in elementContext.Mutations)
                m.MutateText(html);

            TagsRenderer.RenderInto(tags, html);
            return html.ToString();
        }

    }
}
