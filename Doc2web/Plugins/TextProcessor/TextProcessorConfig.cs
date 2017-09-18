using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.TextProcessor
{
    public class TextProcessorConfig
    {
        public double ContainerStart { get; set; }
        public double ContainerEnd { get; set; }
        
        public string ContainerTag { get; set; }

        public string ContainerCls { get; set; }

        public int ContainerZ { get; set; }

        public string IdentationTag { get; set; }

        public double LeftIndentationStart { get; set; }

        public double LeftIndentationEnd { get; set; }

        public string LeftIdentationCls { get; set; }

        public double RightIndentationStart { get; set; }

        public double RightIndentationEnd { get; }

        public string RightIndentationCls { get; set; }

        public string ParagraphTag { get; set; }

        public string ParagraphCls { get; set; }

        public double ParagraphDelta { get; }

        public int ParagraphZ { get; set; }

        public string RunTag { get; set; }

        public string RunCls { get; set; }

        public int RunZ { get; set; }

        public TextProcessorConfig()
        {
            ContainerStart = double.MinValue;
            ContainerEnd = double.MaxValue;
            ContainerTag = "div";
            ContainerCls = "container";
            ContainerZ = 1_000_000;

            IdentationTag = "div";

            LeftIndentationStart = double.MinValue + 0.01;
            LeftIndentationEnd = double.MinValue + 0.02;
            LeftIdentationCls = "leftspacer";

            RightIndentationStart = double.MaxValue - 0.02;
            RightIndentationEnd = double.MaxValue - 0.01;
            RightIndentationCls = "rightspacer";

            ParagraphTag = "p";
            ParagraphCls = "";
            ParagraphDelta = 0.01;
            ParagraphZ = 1_000;
            RunTag = "span";
            RunCls = "";
            RunZ = 1;
        }
    }
}
