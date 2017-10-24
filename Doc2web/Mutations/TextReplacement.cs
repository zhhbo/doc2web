using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web
{
    /// <summary>
    /// Mutation that replaces text into the document.
    /// </summary>
    public class TextReplacement : Mutation
    {
        /// <summary>
        /// Count of character that will be deleted.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Characters that will be inserted.
        /// </summary>
        public string Replacement { get; set; }

        public override double Offset => Math.Max(0, Length - Replacement.Length);

        public override string Render() => Replacement;
    }
}
