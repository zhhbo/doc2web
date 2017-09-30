using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public class StyleConfig
    {
        public string ParagraphCssClassPrefix { get; set; }
        public string RunCssClassPrefix { get; set; }
        public string LeftIdentationCssClassPrefix { get; set; }
        public string RightIdentationCssClassPrefix { get; set; }
        public string DynamicCssClassPrefix { get; set; }

        public StyleConfig()
        {
            ParagraphCssClassPrefix = "div.container";
            RunCssClassPrefix = "span";
            LeftIdentationCssClassPrefix = ".leftspacer";
            RightIdentationCssClassPrefix = ".rightspacer";
            DynamicCssClassPrefix = "dyn";
        }

        public StyleConfig Clone() =>
            new StyleConfig
            {
                ParagraphCssClassPrefix = ParagraphCssClassPrefix,
                RunCssClassPrefix = RunCssClassPrefix,
                LeftIdentationCssClassPrefix = LeftIdentationCssClassPrefix,
                RightIdentationCssClassPrefix = RightIdentationCssClassPrefix
            };

    }
}
