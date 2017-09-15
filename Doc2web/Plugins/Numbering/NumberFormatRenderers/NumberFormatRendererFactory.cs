using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Plugins.Numbering.NumberFormatRenderers
{
    public static class NumberRendererFactory
    {
        public static INumberFormatRenderer Create(NumberFormatValues value)
        {
            switch (value)
            {
                case NumberFormatValues.LowerRoman: return new LowerRomanNumberFormatRenderer();
                case NumberFormatValues.UpperRoman: return new UpperRomanNumberFormatRenderer();
                case NumberFormatValues.LowerLetter: return new LowerLetterNumberFormatRenderer();
                case NumberFormatValues.UpperLetter: return new UpperLetterNumberFormatRenderer();
                case NumberFormatValues.Ordinal: return new OrdinalNumberRenderer();
                case NumberFormatValues.OrdinalText: return new OrdinalTextNumberFormatRenderer();
                case NumberFormatValues.DecimalZero: return new DecimalZeroNumberFormatRenderer();
                default: return new DecimalNumberFormatRendrer();
            }
        }
    }
}
