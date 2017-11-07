using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Doc2web
{
    public class ConversionParameter
    {
        public IEnumerable<OpenXmlElement> Elements { get; set; }

        public virtual Stream Stream { get; set; }

        public virtual bool AutoFlush { get; set; }

        public virtual int AutoFlushBlockCount { get; set; }

        public List<object> AdditionalPlugins { get; set; }

        public bool Parallel { get; set; }

        public ConversionParameter()
        {
            AdditionalPlugins = new List<object>();
        }
    }

}
