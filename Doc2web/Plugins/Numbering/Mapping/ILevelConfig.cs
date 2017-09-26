using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Plugins.Numbering.Mapping
{
    public interface ILevelConfig
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

        ILevelConfig Clone();
        string RenderNumber(int number);
    }
}