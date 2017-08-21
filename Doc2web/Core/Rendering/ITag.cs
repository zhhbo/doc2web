using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Core.Rendering
{
    public interface ITag
    {
        int Index { get; }

        int RelatedIndex { get; }

        int Z { get; }

        string Name { get; }

        string Render();
    }

}
