using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Css
{
    public interface IParagraphClassFactory
    {
        CssClass2 Build(ParagraphClassParam param);
    }
}
