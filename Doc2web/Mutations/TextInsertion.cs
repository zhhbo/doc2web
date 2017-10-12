using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web
{
    /// <summary>
    /// Mutation that insert text into the document.
    /// </summary>
    public class TextInsertion : Mutation
    {
        /// <summary>
        /// Text to be inserted.
        /// </summary>
        public string Text { get; set; }

        public override string Render() => Text;
    }
}
