using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Css
{
    public interface IPropsCache
    {
        CssPropertiesSet Get(string styleId);
    }
}
