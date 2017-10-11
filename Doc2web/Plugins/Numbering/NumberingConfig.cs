using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Numbering
{
    public class NumberingConfig
    {
        public double NumberingIndex { get; set; }
        public double NumberingDelta { get; }

        public string NumberingContainerTag { get; set; }
        public string NumberingContainerCls { get; set; }
        public int NumberingContainerZ { get; set; }

        public string NumberingNumberTag { get; set; }
        public string NumberingNumberCls { get; set; }
        public int NumberingNumberZ { get; set; }


        public NumberingConfig()
        {
            NumberingIndex = -1_000_000 + 0.011;
            NumberingDelta = 0.001;
            NumberingContainerTag = "div";
            NumberingContainerCls = "numbering-container";
            NumberingContainerZ = 900;
            NumberingNumberTag = "span";
            NumberingNumberCls = "numbering-number";
            NumberingNumberZ = 899;
        }
    }
}
