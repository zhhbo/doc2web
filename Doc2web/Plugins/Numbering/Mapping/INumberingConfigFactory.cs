using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Plugins.Numbering
{
    public interface INumberingConfigFactory
    {
        NumberingConfig CreateFromAbstractNumbering(AbstractNum abstractNum);
        NumberingConfig CreateFromNumbering(NumberingConfig abstractNumConfig, NumberingInstance numberingInstance);
    }
}