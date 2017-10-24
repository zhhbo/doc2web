using Doc2web.Core.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web
{
    /// <summary>
    /// Represend a mutation that will be exected on the text of a IElementContext.
    /// This is used to replace some textual content from the open xml.
    /// </summary>
    public abstract class Mutation : IRenderable
    {
        /// <summary>
        /// Position where the mutation will be executed.
        /// </summary>
        public double Position { get; set; }

        /// <summary>
        /// Count of characters to skip.
        /// Node: the posision is a real number, you can squeeze it between text, mutations or HtmlNodes.
        /// </summary>
        public virtual double Offset => 0;

        /// <summary>
        /// Text to be inserted.
        /// </summary>
        public virtual string Render() => "";

    }
}
