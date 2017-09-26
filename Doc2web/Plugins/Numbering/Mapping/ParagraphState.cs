using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Plugins.Numbering.Mapping
{
    public class ParagraphState
    {
        public ParagraphState() { }

        public int NumberingInstanceId { get; set; }

        public IEnumerable<int> Indentations { get; set; }

    }
}
