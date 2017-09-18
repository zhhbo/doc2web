using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Core.Rendering
{
    public interface IRenderable
    {
        string Render();

        double Position { get; set; }

        double Offset { get; }
    }
}
