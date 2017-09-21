using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml;

namespace Doc2web.Plugins.Style
{
    public interface ICssPropertiesFactory
    {
        ICssProperty[] BuildRun(OpenXmlElement element);

        ICssProperty[] BuildParagraph(OpenXmlElement element);

        ICssProperty[] BuildNumbering(OpenXmlElement element);
    }
}
