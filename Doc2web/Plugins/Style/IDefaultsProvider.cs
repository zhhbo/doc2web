using Doc2web.Plugins.Style;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public interface IDefaultsProvider
    {
        CssPropertiesSet Paragraph { get; }
        CssPropertiesSet Run { get; }
    }
}
