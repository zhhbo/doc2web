using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web
{
    /// <summary>
    /// Mutation that deleted some text from the document.
    /// </summary>
    public class TextDeletion : Mutation
    {
        /// <summary>
        /// Number of character to be deleted
        /// </summary>
        public int Count { get; set; }

        public override double Offset => Count;
    }
}
