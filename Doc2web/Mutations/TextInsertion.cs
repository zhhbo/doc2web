using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web
{
    public class TextInsertion : Mutation
    {
        public string Text { get; set; }

        public override string Render() => Text;
    }
}
