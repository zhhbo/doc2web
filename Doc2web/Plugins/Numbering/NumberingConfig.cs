using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Numbering
{
    /// <summary>
    /// Plugin that will generate the numbering into the html document.
    /// </summary>
    public class NumberingConfig
    {
        /// <summary>
        /// Position where the numbering should be starting.
        /// </summary>
        public double NumberingStartingPosition { get; set; }

        /// <summary>
        /// Distance between each HtmlNode used to produce the numbering.
        /// </summary>
        public double NumberingDelta { get; }

        /// <summary>
        /// Key used to stored the numbering paragraph data in the IElementContext.ViewBag.
        /// </summary>
        public string ParagraphNumberingDataKey { get; set; }

        /// <summary>
        /// Element name that will be containing the margin for the numbering.
        /// </summary>
        public string NumberingContainerTag { get; set; }

        /// <summary>
        /// Css class that will be added to the HtmlNode containing the margin for the numbering.
        /// </summary>
        public string NumberingContainerCls { get; set; }

        /// <summary>
        /// Z index of the HtmlNode that will be used to contains the margin for the numbering. 
        /// </summary>
        public int NumberingContainerZ { get; set; }

        /// <summary>
        /// Tag of the HtmlNode containing the numbering.
        /// </summary>
        public string NumberingNumberTag { get; set; }

        /// <summary>
        /// Css class that will be added to the HtmlNode containing the numbering.
        /// </summary>
        public string NumberingNumberCls { get; set; }
        
        /// <summary>
        /// Z index of the HtmlNode containing the numbering.
        /// </summary>
        public int NumberingNumberZ { get; set; }


        public NumberingConfig()
        {
            ParagraphNumberingDataKey = "numbering";
            NumberingStartingPosition = -1_000_000 + 0.011;
            NumberingDelta = 0.001;
            NumberingContainerTag = "div";
            NumberingContainerCls = "leftspacer";
            NumberingContainerZ = 900;
            NumberingNumberTag = "span";
            NumberingNumberCls = "numbering-number";
            NumberingNumberZ = 899;
        }
    }
}
