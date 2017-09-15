using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Plugins.Numbering.NumberFormatRenderers
{
    /// <summary>
    /// Render a number into the roman format. Uses upper case.
    /// </summary>
    public class UpperRomanNumberFormatRenderer : LowerRomanNumberFormatRenderer
    {
        public override string Render(int value) =>
          base.Render(value).ToUpperInvariant();

    }
}
