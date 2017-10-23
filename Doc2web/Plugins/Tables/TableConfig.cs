using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Tables
{
    public class TableConfig
    {
        public string WarningMessage { get; set; }

        public string WarningCssClass { get; set; }

        public TableConfig()
        {
            WarningMessage = @"Doc2web does not support tables yet.";
            WarningCssClass = "table-not-supported";
        }
    }
}
