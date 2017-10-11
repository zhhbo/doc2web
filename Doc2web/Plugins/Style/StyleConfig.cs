using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public class StyleConfig
    {
        public string ContainerCssClassPrefix { get; set; }
        public string ContainerWithNumberingCssClassSuffix { get; set; }
        public string NumberingNumber { get; set; }

        public string RunCssClassPrefix { get; set; }
        public string ParagraphCssClassPrefix { get; }
        public string LeftIdentationCssClassPrefix { get; set; }
        public string RightIdentationCssClassPrefix { get; set; }
        public string DynamicCssClassPrefix { get; set; }

        public StyleConfig()
        {
            ContainerCssClassPrefix = "div.container";
            ContainerWithNumberingCssClassSuffix = ".numbering";
            NumberingNumber = ".numbering-number";
            RunCssClassPrefix = "span";
            ParagraphCssClassPrefix = "p";
            LeftIdentationCssClassPrefix = ".leftspacer";
            RightIdentationCssClassPrefix = ".rightspacer";
            DynamicCssClassPrefix = "d";
        }

        public StyleConfig Clone() =>
            new StyleConfig
            {
                ContainerCssClassPrefix = ContainerCssClassPrefix,
                ContainerWithNumberingCssClassSuffix = ContainerWithNumberingCssClassSuffix,
                RunCssClassPrefix = RunCssClassPrefix,
                LeftIdentationCssClassPrefix = LeftIdentationCssClassPrefix,
                RightIdentationCssClassPrefix = RightIdentationCssClassPrefix
            };

    }
}
