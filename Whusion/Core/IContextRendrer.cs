using System;
using System.Collections.Generic;
using System.Text;

namespace Whusion.Core
{
    public interface IContextRenderer
    {
        string Render(IElementContext elementContext);
    }
}
