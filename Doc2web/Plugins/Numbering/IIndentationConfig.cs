using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Plugins.Numbering
{
    public interface IIndentationConfig
    {
        bool IsFromAbstract { get; set; }
        int LevelIndex { get; set; }
        Level LevelNode { get; set; }
        int NumberId { get; set; }
        NumberFormatValues NumberingFormat { get; set; }
        string ParagraphStyleId { get; set; }
        int StartValue { get; set; }
        string Text { get; set; }
        bool ForceNumbericRendering { get; set; }

        IIndentationConfig Clone();
        string RenderNumber(int number);
    }
}