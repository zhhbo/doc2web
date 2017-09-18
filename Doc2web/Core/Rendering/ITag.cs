using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Core.Rendering
{
    public interface ITag
    {
        double Index { get; set; }

        double RelatedIndex { get; }

        double Z { get; }

        string Name { get; }

        string Render();
    }

}
