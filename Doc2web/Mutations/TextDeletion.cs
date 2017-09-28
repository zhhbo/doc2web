using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web
{
    public class TextDeletion : Mutation
    {
        public int Count { get; set; }

        public override double Offset => Count;
    }
}
