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

        public string NumberingContainerMaxCls { get; set; }
        public string NumberingContainerMinCls { get; set; }

        public int NumberingContainerZ { get; set; }
        public string NumberingNumberTag { get; set; }

        public string NumberingNumberMaxCls { get; set; }
        public string NumberingNumberMinCls { get; set; }
        public int NumberingNumberZ { get; set; }


        public NumberingConfig()
        {
            NumberingIndex = -1_000_000 + 0.011;
            NumberingDelta = 0.001;
            NumberingContainerTag = "div";
            NumberingContainerMaxCls = "numbering-container-max";
            NumberingContainerMinCls = "numbering-container-min";
            NumberingContainerZ = 900;
            NumberingNumberTag = "span";
            NumberingNumberMaxCls = "numbering-number-max";
            NumberingNumberMinCls = "numbering-number-min";
            NumberingNumberZ = 899;
        }
    }
}
