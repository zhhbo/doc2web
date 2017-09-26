using System;

namespace Doc2web.Plugins.Numbering.Stringifiers
{
    /// <summary>
    /// Render number into a decimal string (AKA ToString method);
    /// </summary>
    public class DecimalStringifier : IStringifier
    {
        public string Render(int value) => value.ToString();
    }
}