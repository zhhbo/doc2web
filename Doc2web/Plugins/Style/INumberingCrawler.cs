using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public interface INumberingCrawler
    {
        List<Level> Collect(int numberingId, int level);
    }
}
