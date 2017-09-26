using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Doc2web.Plugins.Numbering.Stringifiers;

namespace Doc2web.Plugins.Numbering
{
    public class IndentationConfig : IIndentationConfig
    {
        public int NumberId { get; set; }
        public int LevelIndex { get; set; }
        public bool IsFromAbstract { get; set; }
        public bool ForceNumbericRendering { get; set; }

        public int StartValue { get; set; }
        public NumberFormatValues NumberingFormat { get; set; }
        public string Text { get; set; }
        public string ParagraphStyleId { get; set; }

        public Level LevelNode { get; set; }

        public IIndentationConfig Clone() => new IndentationConfig
        {
            NumberId = NumberId,
            LevelIndex = LevelIndex,
            IsFromAbstract = IsFromAbstract,
            StartValue = StartValue,
            NumberingFormat = NumberingFormat,
            Text = Text,
            ParagraphStyleId = ParagraphStyleId,
            LevelNode = LevelNode
        };

        public string RenderNumber(int number) =>
          StringifierFactory.Create(NumberingFormat).Render(number);
    }
}