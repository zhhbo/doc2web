using Doc2web.Core.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web
{
    public abstract class Mutation : IRenderable
    {
        public double Position { get; set; }

        public virtual double Offset => 0;

        public virtual string Render() => "";

    }
}
