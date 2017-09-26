using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Plugins.Numbering.Mapping
{
    public class ParagraphData
    {
        private Config nconf;
        private ParagraphState state;

        public ParagraphData(Config nconf, ParagraphState state)
        {
            this.nconf = nconf;
            this.state = state;
        }

        public int NumberingId => state.NumberingInstanceId;
        public int LevelIndex => state.Indentations.Count() - 1;
        public Level LevelXmlElement => nconf[LevelIndex].LevelNode;
        public string Verbose => nconf.Render(state.Indentations);
        public string Location =>
          String.Join(" &rarr; ",
            Enumerable.Range(1, state.Indentations.Count())
            .Select(i => nconf.Render(state.Indentations.ToList().GetRange(0, i))));
    }
}
