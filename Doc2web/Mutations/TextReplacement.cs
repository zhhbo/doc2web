using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web
{
    public class TextReplacement : Mutation
    {
        public int Length { get; set; }

        public string Replacement { get; set; }

        public override double Offset => Math.Max(0, Length - Replacement.Length);

        public override string Render() => Replacement;
    }
}
