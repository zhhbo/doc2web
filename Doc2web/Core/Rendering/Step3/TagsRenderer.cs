using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Core.Rendering.Step3
{
    public static class TagsRenderer
    {
        public static void RenderInto(ITag[] tags, StringBuilder sb)
        {
            int offset = 0;

            foreach(var tag in tags)
            {
                var html = tag.Render();
                sb.Insert(tag.Index + offset, html);
                offset += html.Length;
            }
        }
    }
}
