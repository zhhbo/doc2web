using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Plugins.Numbering.Mapping.Stringifiers
{
    /// <summary>
    /// Provider a method to render a number into a specific format.
    /// </summary>
    public interface IStringifier
    {
        /// <summary>
        /// Render a number into a formatted string.
        /// </summary>
        /// <param name="value">Targeted number</param>
        /// <returns>Formated string representing the number</returns>
        string Render(int value);
    }
}
