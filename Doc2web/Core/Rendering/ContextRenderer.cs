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
            (List<HtmlNode> nodes, Mutation[] mutations) = BuildNodes(elementContext);
            ITag[] tags = BuildTags(nodes);
            return Render(elementContext.RootElement.InnerText, mutations, tags);
        }

        public static (List<HtmlNode>, Mutation[]) BuildNodes(IElementContext elementContext)
        {
            var nodes = elementContext.Nodes.ToList();
            var mutations = elementContext.Mutations.ToList();

            MutationsApplier.Apply(nodes, mutations);

            return (FlatternHtmlNodes.Flattern(nodes), mutations.ToArray());
        }

        public static ITag[] BuildTags(List<HtmlNode> nodes)
        {
            return TagsFactory.Build(nodes);
        }

        public static string Render(string text, Mutation[] mutations, ITag[] tags)
        {
            var html = new StringBuilder(text);

            for(int i=0; i<mutations.Length; i++)
                mutations[i].MutateText(html);

            TagsRenderer.RenderInto(tags, html);

            return html.ToString();
        }

    }
}
