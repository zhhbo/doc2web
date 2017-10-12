using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.TextFixes
{
    public class BreakInsertionConfig
    {
        /// <summary>
        /// Tag of HtmlNode wrapping up a RootElement.
        /// </summary>
        public string ContainerTag { get; set; }

        /// <summary>
        /// Css class that has to be on HtmlNode wrapping up RootElements.
        /// </summary>
        public string ContainerCls { get; set; }

        /// <summary>
        /// Z index of HtmlNode wrapping up RootElement.
        /// </summary>
        public int ContainerZ { get; set; }

        /// <summary>
        /// Css class that has to be on HtmlNode creating margin for numbering.
        /// </summary>
        public string LeftSpacerCls { get; set; }

        /// <summary>
        /// Css class that will be added to HtmlNode that will be wrapping up RootElements 
        /// when there is a Br tag at it's start.
        /// </summary>
        public string BreakAtStartCls { get; set; }


        public BreakInsertionConfig()
        {
            ContainerTag = "div";
            ContainerCls = "container";
            ContainerZ = 1_000_000;
            LeftSpacerCls = "leftspacer";
            BreakAtStartCls = "break-at-start";
        }
    }
}
