using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Core.Rendering.Step3
{
    public static class TagsRenderer
    {
        public static string Render(ITag[] tags, StringBuilder sb)
        {
            int totalOffset = 0;
            (int, string)[] insertions = BuildInsertions(tags, ref totalOffset);

            char[] result = new char[totalOffset + sb.Length];
            int lastIndex = 0;
            int top = 0;

            for (int i = 0; i < insertions.Length; i++)
            {
                var (htmlIndex, html) = insertions[i];
                sb.CopyTo(lastIndex, result, top, htmlIndex - lastIndex);
                top += htmlIndex - lastIndex;

                top = InsertHtml(result, top, html);
                lastIndex = htmlIndex;
            }

            sb.CopyTo(lastIndex, result, top, sb.Length - lastIndex);
            return new string(result);
        }

        private static int InsertHtml(char[] result, int top, string html)
        {
            for (int j = 0; j < html.Length; j++)
                result[top + j] = html[j];
            top += html.Length;
            return top;
        }

        private static (int, string)[] BuildInsertions(ITag[] tags, ref int totalOffset)
        {
            (int, string)[] insertions = new(int, string)[tags.Length];
            for (int i = 0; i < tags.Length; i++)
            {
                var tag = tags[i];
                string html = tag.Render();
                insertions[i] = (tag.Index, html);
                totalOffset += html.Length;
            }
            return insertions;
        }
    }
}
