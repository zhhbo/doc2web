using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml;

namespace Doc2web.Plugins.Style
{
    public interface ICssPropertiesFactory2
    {
        ICssProperty[] Build(OpenXmlElement element);
    }
}
