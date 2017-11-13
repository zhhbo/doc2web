using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Doc2web
{
    public class ConversionParameter
    {
        /// <summary>
        /// Elements to be considered as RootElements in the conversion task.
        /// </summary>
        public IEnumerable<OpenXmlElement> Elements { get; set; }

        /// <summary>
        /// Stream where the html will be written.
        /// </summary>
        public virtual Stream Stream { get; set; }

        /// <summary>
        /// AutoFlush paramter on the stream writter. If true, html will be written sequentialy on the stream.
        /// If false, the html will be buffered before the writting.
        /// </summary>
        public virtual bool AutoFlush { get; set; }


        /// <summary>
        /// Plugin objects that will be added for this conversion task.
        /// </summary>
        public List<object> AdditionalPlugins { get; set; }

        public ConversionParameter()
        {
            AdditionalPlugins = new List<object>();
        }
    }

}
