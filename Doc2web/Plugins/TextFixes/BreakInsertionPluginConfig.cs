using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.TextFixes
{
    public class BreakInsertionPluginConfig
    {
        public string ContainerTag { get; set; }
        public string ContainerCls { get; set; }
        public int ContainerZ { get; set; }

        public BreakInsertionPluginConfig()
        {
            ContainerTag = "div";
            ContainerCls = "container";
            ContainerZ = 1_000_000;
        }
    }
}
