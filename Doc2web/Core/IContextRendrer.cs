using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Core
{
    public interface IContextRenderer
    {
        string Render(IElementContext elementContext);
    }
}
