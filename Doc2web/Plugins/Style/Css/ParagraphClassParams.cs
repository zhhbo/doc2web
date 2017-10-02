using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Css
{
    public class ParagraphClassParams
    {
        public string StyleId { get; set; }

        public OpenXmlElement InlineProperties { get; set; }

        public int? NumberingId { get; set; }

        public int? NumberingLevel { get; set; }
    }
}
