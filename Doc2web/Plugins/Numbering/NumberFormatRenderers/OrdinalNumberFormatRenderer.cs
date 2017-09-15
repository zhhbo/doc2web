using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Plugins.Numbering.NumberFormatRenderers
{
    /// <summary>
    /// Append the ordinal format to a number. Ex: 1st, 2nd, 3rd, 4th, etc...
    /// </summary>
    public class OrdinalNumberRenderer : INumberFormatRenderer
    {
        public string Render(int value)
        {
            int subValue = value;
            if (value > 20)
            {
                subValue = value % 10;
            }
            switch (subValue)
            {
                case 1: return value + "st";
                case 2: return value + "nd";
                case 3: return value + "rd";
                default: return value + "th";
            }
        }
    }
}
