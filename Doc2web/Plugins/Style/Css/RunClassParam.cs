using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Css
{
    public class RunClassParam
    {
        public string ParagraphStyleId { get; set; }

        public string RunStyleId { get; set; }

        public int NumberingId { get; set; }

        public int NumberingLevel { get; set; }

        public OpenXmlElement InlineProps { get; set; }
    }
}
