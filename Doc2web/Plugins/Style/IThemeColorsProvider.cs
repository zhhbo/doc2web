using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins
{
    public interface IThemeColorsProvider
    {
        string GetColor(ThemeColorValues value);
    }
}
