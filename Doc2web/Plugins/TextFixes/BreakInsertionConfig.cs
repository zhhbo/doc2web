using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.TextFixes
{
    public class BreakInsertionConfig
    {
        public string ContainerTag { get; set; }
        public string ContainerCls { get; set; }
        public int ContainerZ { get; set; }
        public string LeftSpacerCls { get; set; }
        public string BreakAtStartCls { get; set; }
        public string NumberingContainerCls { get; set; }

        public BreakInsertionConfig()
        {
            ContainerTag = "div";
            ContainerCls = "container";
            ContainerZ = 1_000_000;
            LeftSpacerCls = "leftspacer";
            NumberingContainerCls = "numbering-container-min";
            BreakAtStartCls = "break-at-start";
        }
    }
}
