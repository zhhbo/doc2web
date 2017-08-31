using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public interface ICssClass
    {
        CssData AsCss();

        string Selector { get; set; }

        bool IsEmpty { get; }
    }
}
