using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public class StyleConfiguration
    {
        public string ParagraphCssClassPrefix { get; set; }
        public string RunCssClassPrefix { get; set; }
        public string LeftIdentationCssClassPrefix { get; set; }
        public string RightIdentationCssClassPrefix { get; set; }

        public StyleConfiguration()
        {
            ParagraphCssClassPrefix = "div.container";
            RunCssClassPrefix = "span";
            LeftIdentationCssClassPrefix = ".leftspacer";
            RightIdentationCssClassPrefix = ".rightspacer";
        }

        public StyleConfiguration Clone() =>
            new StyleConfiguration
            {
                ParagraphCssClassPrefix = ParagraphCssClassPrefix,
                RunCssClassPrefix = RunCssClassPrefix,
                LeftIdentationCssClassPrefix = LeftIdentationCssClassPrefix,
                RightIdentationCssClassPrefix = RightIdentationCssClassPrefix
            };

    }
}
