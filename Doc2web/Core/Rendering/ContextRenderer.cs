using Doc2web.Core.Rendering;
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
            HtmlNode[] nodes = Step1(elementContext);
            ITag[] tags = Step2(nodes);

            var renderables =
                tags.Cast<IRenderable>()
                .Concat(elementContext.Mutations.Cast<IRenderable>())
                .ToArray();
            return Step3(
                elementContext.RootElement.InnerText,
                renderables
            );
        }

        public static HtmlNode[] Step1(IElementContext elementContext)
        {
            var nodes = elementContext.Nodes.ToList();
            HtmlNodesFlatterner.Flattern(nodes);
            return nodes.ToArray();
        }

        public static ITag[] Step2(HtmlNode[] nodes) =>
           TagsFactory.Build(nodes);

        public static string Step3(string text, IRenderable[] renderables) =>
            new Stringifier() {
                Text = text,
                Elements = renderables
            }.Stringify();

    }
}
