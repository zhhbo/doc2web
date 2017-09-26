using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Plugins.Numbering.Mapping.Stringifiers
{
    /// <summary>
    /// Render numbers like a = 1, b = 2, aa = 27, bb = 28, etc...
    /// Uses lower cases.
    /// </summary>
    public class LowerLetterStringifier : IStringifier
    {
        private static char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLowerInvariant().ToCharArray();

        public virtual string Render(int value)
        {
            value--;
            char letter = alpha[(value % alpha.Length)];
            double dval = value + 1;
            double dlength = alpha.Length;
            int count = (int)Math.Ceiling(dval / dlength);
            return String.Concat(Enumerable.Repeat(letter, count));
        }
    }
}
