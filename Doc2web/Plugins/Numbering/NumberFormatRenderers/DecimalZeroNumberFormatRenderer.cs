using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Plugins.Numbering.NumberFormatRenderers
{
    public class DecimalZeroNumberFormatRenderer : INumberFormatRenderer
    {
        public string Render(int value)
        {
            if (value < 10)
                return "0" + value.ToString();
            else
                return value.ToString();
        }
    }
}
