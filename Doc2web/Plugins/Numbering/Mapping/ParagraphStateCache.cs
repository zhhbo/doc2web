using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Plugins.Numbering
{
    public class ParagraphStateCache
    {
        private Dictionary<int, ParagraphState> _cache = new Dictionary<int, ParagraphState>();


        public ParagraphState Get(Paragraph paragraph)
        {
            ParagraphState output = null;
            _cache.TryGetValue(paragraph.GetHashCode(), out output);
            return output;
        }

        public void Add(Paragraph p, ParagraphState state)
        {
            _cache.Add(p.GetHashCode(), state);
        }
    }
}
