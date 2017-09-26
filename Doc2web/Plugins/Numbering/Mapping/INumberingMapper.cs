using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Plugins.Numbering.Mapping
{
    public interface INumberingMapper
    {
        bool IsValid { get; }

        IParagraphData GetNumbering(Paragraph p);
    }
}