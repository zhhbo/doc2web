using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Plugins.Numbering
{
    public interface IConfigFactory
    {
        Config CreateFromAbstractNumbering(AbstractNum abstractNum);
        Config CreateFromNumbering(Config abstractNumConfig, NumberingInstance numberingInstance);
    }
}