using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Plugins.Numbering.Mapping.Stringifiers
{
    public static class StringifierFactory
    {
        public static IStringifier Create(NumberFormatValues value)
        {
            switch (value)
            {
                case NumberFormatValues.LowerRoman: return new LowerRomanStringifier();
                case NumberFormatValues.UpperRoman: return new UpperRomanStringifier();
                case NumberFormatValues.LowerLetter: return new LowerLetterStringifier();
                case NumberFormatValues.UpperLetter: return new UpperLetterStringifier();
                case NumberFormatValues.Ordinal: return new OrdinalNumberStringifier();
                case NumberFormatValues.OrdinalText: return new OrdinalTextStringifier();
                case NumberFormatValues.DecimalZero: return new DecimalZeroStringifier();
                case NumberFormatValues.None: return new NoneStringifier();
                default: return new DecimalStringifier();
            }
        }
    }
}
