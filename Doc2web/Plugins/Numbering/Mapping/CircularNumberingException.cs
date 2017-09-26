using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Plugins.Numbering.Mapping
{
    /// <summary>
    /// Exception that happens when a numbering instance refers an abstract numbering and this
    /// abstract numbering refers a style that refers the same numbering instance.
    /// 1 (num id) -> 2 (anum id) -> styleid (style id) -> 1 (num id)
    /// </summary>
    public class CircularNumberingException : Exception
    {
    }
}
