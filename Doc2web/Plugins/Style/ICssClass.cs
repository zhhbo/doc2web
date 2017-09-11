using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public interface ICssClass
    {
        void InsertCss(CssData cssData);

        string Selector { get; set; }

        bool IsEmpty { get; }
    }
}
