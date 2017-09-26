using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Plugins.Numbering.Mapping
{
    public interface IConfigFactory
    {
        Config CreateFromAbstractNumbering(AbstractNum abstractNum);
        Config CreateFromNumbering(Config abstractNumConfig, NumberingInstance numberingInstance);
    }
}