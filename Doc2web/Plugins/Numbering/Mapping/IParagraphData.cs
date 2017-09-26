using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Numbering.Mapping
{
    public interface IParagraphData
    {
        int NumberingId { get; }

        int LevelIndex { get; }

        Level LevelXmlElement { get; } 

        string Verbose { get; }

        string Location { get; }
    }
}
