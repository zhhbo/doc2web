using System;

namespace Doc2web.Plugins.Numbering.NumberFormatRenderers
{
    /// <summary>
    /// Render numbers like A = 1, B = 2, AA = 27, BB = 28, etc...
    /// Uses upper cases.
    /// </summary>
    public class UpperLetterNumberFormatRenderer : LowerLetterNumberFormatRenderer
    {
        public override string Render(int value) => base.Render(value).ToUpperInvariant();
    }
}