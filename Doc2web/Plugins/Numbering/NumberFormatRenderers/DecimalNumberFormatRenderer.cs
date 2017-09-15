using System;

namespace Doc2web.Plugins.Numbering.NumberFormatRenderers
{
    /// <summary>
    /// Render number into a decimal string (AKA ToString method);
    /// </summary>
    public class DecimalNumberFormatRendrer : INumberFormatRenderer
    {
        public string Render(int value) => value.ToString();
    }
}