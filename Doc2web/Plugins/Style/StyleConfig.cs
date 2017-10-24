using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public class StyleConfig
    {
        /// <summary>
        /// Css prefix that will be used on all classes targetin RootElement.
        /// </summary>
        public string ContainerCssClassPrefix { get; set; }

        /// <summary>
        /// Css suffix that will be added on classes targering RootElements that has numbering.
        /// </summary>
        public string ContainerWithNumberingCssClassSuffix { get; set; }

        /// <summary>
        /// Css selector that will used to target the automated numbering numbers.
        /// </summary>
        public string NumberingNumberSelector { get; set; }

        /// <summary>
        /// Css prefix that will be added on classes targeting runs.
        /// </summary>
        public string RunCssClassPrefix { get; set; }

        /// <summary>
        /// Css prefix that will be added on classes targeting paragraphs.
        /// </summary>
        public string ParagraphCssClassPrefix { get; }

        /// <summary>
        /// Css suffix that will be added target spacers that are added for the numbering.
        /// </summary>
        public string LeftIdentationCssClassSuffix { get; set; }
        
        /// <summary>
        /// Prefix that will be added to all class names that are generated dynamicly.
        /// </summary>
        public string DynamicCssClassPrefix { get; set; }

        public StyleConfig()
        {
            ContainerCssClassPrefix = "div.container";
            ContainerWithNumberingCssClassSuffix = ".numbering";
            NumberingNumberSelector = ".numbering-number";
            RunCssClassPrefix = "span";
            ParagraphCssClassPrefix = "p";
            LeftIdentationCssClassSuffix = ".leftspacer";
            DynamicCssClassPrefix = "d";
        }

        public StyleConfig Clone() =>
            new StyleConfig
            {
                ContainerCssClassPrefix = ContainerCssClassPrefix,
                ContainerWithNumberingCssClassSuffix = ContainerWithNumberingCssClassSuffix,
                RunCssClassPrefix = RunCssClassPrefix,
                LeftIdentationCssClassSuffix = LeftIdentationCssClassSuffix,
            };

    }
}
