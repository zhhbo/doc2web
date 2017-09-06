using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Plugins.Style
{
    public interface IThemeFontsProvider
    {
        string GetFontFace(ThemeFontValues value);
    }
}