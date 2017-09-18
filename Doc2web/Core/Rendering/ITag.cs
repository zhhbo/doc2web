using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Core.Rendering
{
    public interface ITag : IRenderable
    {
        double RelatedPosition { get; }

        string Name { get; }
    }

}
