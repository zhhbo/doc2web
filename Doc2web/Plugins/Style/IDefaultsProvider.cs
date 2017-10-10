using Doc2web.Plugins.Style;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public interface IDefaultsProvider
    {
        string DefaultParagraphStyle { get; }

        CssPropertiesSet Paragraph { get; }

        string DefaultRunStyle { get; }

        CssPropertiesSet Run { get; }
    }
}
