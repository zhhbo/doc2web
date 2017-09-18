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
            HtmlNode[] nodes = BuildNodes(elementContext);

            ITag[] tags = BuildTags(nodes);
            return Render(elementContext.RootElement.InnerText, elementContext.Mutations.ToArray(), tags);
        }

        public static HtmlNode[] BuildNodes(IElementContext elementContext)
        {
            var nodes = elementContext.Nodes.ToList();

            FlatternHtmlNodes.Apply(nodes);

            return nodes.ToArray();
        }

        public static ITag[] BuildTags(HtmlNode[] nodes)
        {
            return TagsFactory.Build(nodes);
        }

        public static string Render(string text, Mutation[] mutations, ITag[] tags)
        {
            var renderer = new Renderer2()
            {
                Text = text,
                Elements =
                    tags
                    .Cast<IRenderable>()
                    .Concat(mutations.Cast<IRenderable>())
                    .ToArray()
            };

            return renderer.Render();
        }

    }
}
