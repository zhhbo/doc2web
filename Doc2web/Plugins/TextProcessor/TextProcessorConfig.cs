using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.TextProcessor
{
    public class TextProcessorConfig
    {
        public string PPropsCssClassKey { get; set; }

        public double ContainerStart { get; set; }

        public double ContainerEnd { get; set; }
        
        public string ContainerTag { get; set; }

        public string ContainerCls { get; set; }

        public string ContainerWithNumberingCls { get; set; }

        public int ContainerZ { get; set; }

        public string IdentationTag { get; set; }

        public string LeftIdentationCls { get; set; }

        public string RightIndentationCls { get; set; }

        public string ParagraphTag { get; set; }

        public string ParagraphCls { get; set; }

        public double Delta { get; }

        public int ParagraphZ { get; set; }

        public string RunTag { get; set; }

        public string RunCls { get; set; }

        public int RunZ { get; set; }

        public TextProcessorConfig()
        {
            PPropsCssClassKey = "pPr";

            ContainerStart = -1_000_000;
            ContainerEnd = 1_000_000;
            ContainerTag = "div";
            ContainerCls = "container";
            ContainerWithNumberingCls = "numbering";
            ContainerZ = 1_000_000;

            IdentationTag = "div";

            LeftIdentationCls = "leftspacer";

            RightIndentationCls = "rightspacer";

            ParagraphTag = "p";
            ParagraphCls = "";
            Delta = 0.01;
            ParagraphZ = 1_000;
            RunTag = "span";
            RunCls = "";
            RunZ = 1;
        }
    }
}
