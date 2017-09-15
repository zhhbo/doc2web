using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Plugins.Numbering.NumberFormatRenderers
{
    /// <summary>
    /// Render a number into the roman format. Uses lower case.
    /// </summary>
    public class LowerRomanNumberFormatRenderer : INumberFormatRenderer
    {
        public virtual string Render(int value)
        {
            List<string> accumulator = new List<string>();
            while (value > 0)
            {
                if (value >= 900)
                {
                    value -= 900;
                    accumulator.Add("cm");
                }
                else if (value >= 500)
                {
                    value -= 500;
                    accumulator.Add("d");
                }
                else if (value >= 400)
                {
                    value -= 400;
                    accumulator.Add("cd");
                }
                else if (value >= 100)
                {
                    value -= 100;
                    accumulator.Add("c");
                }
                else if (value >= 90)
                {
                    value -= 90;
                    accumulator.Add("xc");
                }
                else if (value >= 50)
                {
                    value -= 50;
                    accumulator.Add("l");
                }
                else if (value >= 40)
                {
                    value -= 40;
                    accumulator.Add("xl");
                }
                else if (value >= 10)
                {
                    value -= 10;
                    accumulator.Add("x");
                }
                else if (value >= 9)
                {
                    value -= 9;
                    accumulator.Add("ix");
                }
                else if (value >= 5)
                {
                    value -= 5;
                    accumulator.Add("v");
                }
                else if (value >= 4)
                {
                    value -= 4;
                    accumulator.Add("iv");
                }
                else if (value >= 1)
                {
                    value -= 1;
                    accumulator.Add("i");
                }
            }
            return String.Concat(accumulator);
        }
    }
}
