using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.TextProcessor
{
    public class TextProcessorConfig
    {
        /// <summary>
        /// Key that will be used to store the CssClass created from a ParagraphProperties.
        /// </summary>
        public string PPropsCssClassKey { get; set; }

        /// <summary>
        /// Starting position of the HtmlNode wrapping up a RootElement.
        /// </summary>
        public double ContainerStart { get; set; }

        /// <summary>
        /// Ending position of the HtmlNode wrapping up a RootElement.
        /// </summary>
        public double ContainerEnd { get; set; }
        
        /// <summary>
        /// Tag of the HtmlNode wrapping up a RootElement.
        /// </summary>
        public string ContainerTag { get; set; }

        /// <summary>
        /// Css class that will be added to the HtmlNode wrapping up a RootElement.
        /// </summary>
        public string ContainerCls { get; set; }

        /// <summary>
        /// Css class that will be added to the HtmlNode wrapping up a RootElement with numbering.
        /// </summary>
        public string ContainerWithNumberingCls { get; set; }

        /// <summary>
        /// Z index of the HtmlNode wrapping up a RootElement.
        /// </summary>
        public int ContainerZ { get; set; }

        /// <summary>
        /// Tag of HtmlNode used to create a spacer from the indentation to position the numbering.
        /// </summary>
        public string IdentationTag { get; set; }

        /// <summary>
        /// Css class that will be added to the left spacer for the numbering
        /// </summary>
        public string LeftIdentationCls { get; set; }

        /// <summary>
        /// Tag of the HtmlNode that will be representing a paragraph. 
        /// </summary>
        public string ParagraphTag { get; set; }

        /// <summary>
        /// Css class that will be added to the HtmlNode representing a paragraph. 
        /// </summary>
        public string ParagraphCls { get; set; }

        /// <summary>
        /// Distance between the container and spacers HtmlNodes.
        /// </summary>
        public double Delta { get; }

        /// <summary>
        /// Z index of the HtmlNode representing a paragraph.
        /// </summary>
        public int ParagraphZ { get; set; }

        /// <summary>
        /// Tag of HtmlNode representing a Run.
        /// </summary>
        public string RunTag { get; set; }

        /// <summary>
        /// Css class that will be added to HtmlNode representing a Run.
        /// </summary>
        public string RunCls { get; set; }

        /// <summary>
        /// Z index of HtmlNodes representing a Run.
        /// </summary>
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
