using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Plugins.Numbering
{
    public class ParagraphNumberingStateCache
    {
        private Dictionary<int, ParagraphNumberingState> _cache = new Dictionary<int, ParagraphNumberingState>();


        public ParagraphNumberingState Get(Paragraph paragraph)
        {
            ParagraphNumberingState output = null;
            _cache.TryGetValue(paragraph.GetHashCode(), out output);
            return output;
        }

        public void Add(Paragraph p, ParagraphNumberingState state)
        {
            _cache.Add(p.GetHashCode(), state);
        }
    }
}
