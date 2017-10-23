using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.TableOfContent
{
    public class TableOfContentConfig
    {
        public double ParagraphStart { get; set; }

        public string ParagraphTag { get; set; }

        public string ParagraphTocCssClass { get; set; }

        public string SpacerTag { get; set; }

        public string SpacerCssClass { get; set; }

        public int SpacerZ { get; set; }

        public TableOfContentConfig()
        {
            ParagraphTag = "p";
            ParagraphStart = 0.0;
            ParagraphTocCssClass = "toc-item";
            SpacerTag = "span";
            SpacerCssClass = "toc-spacer";
            SpacerZ = 2;
        }
    }
}
