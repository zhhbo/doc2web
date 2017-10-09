using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Plugins.Style.Properties
{
    [RunCssProperty(typeof(RunFonts))]
    public class RunFontsCssProperty : CssProperty<RunFonts>
    {
        private IThemeFontsProvider _themeFontProvider;
        private string[] _inlineFonts = new string[4];
        private string[] _themeFonts = new string[4];

        public RunFontsCssProperty(IThemeFontsProvider themeFontProvider)
        {
            _themeFontProvider = themeFontProvider;
        }

        private RunFontsCssProperty() { }

        public override OpenXmlElement OpenXmlElement
        {
            get => base.OpenXmlElement;
            set
            {
                base.OpenXmlElement = value;
                ExtractFonts();
            }
        }

        private void SneakySetElement(OpenXmlElement elem)
        {
            base.OpenXmlElement = elem;
        }

        private void ExtractFonts()
        {
            _inlineFonts[0] = Element.Ascii?.Value;
            _inlineFonts[1] = Element.HighAnsi?.Value;
            _inlineFonts[2] = Element.EastAsia?.Value;
            _inlineFonts[3] = Element.ComplexScript?.Value;

            _themeFonts[0] = TryGetFontFromTheme(Element.AsciiTheme?.Value);
            _themeFonts[1] = TryGetFontFromTheme(Element.HighAnsiTheme?.Value);
            _themeFonts[2] = TryGetFontFromTheme(Element.EastAsiaTheme?.Value);
            _themeFonts[3] = TryGetFontFromTheme(Element.ComplexScriptTheme?.Value);
        }

        private string TryGetFontFromTheme(ThemeFontValues? value)
        {
            try
            {
                if (!value.HasValue) return null;
                return _themeFontProvider.GetFontFace(value.Value);
            }
            catch
            {
                return null;
            }
        }

        public override void InsertCss(CssData cssData)
        {
            string fontFamily = CreateFontFamilyValue();
            if (fontFamily.Length == 0) return;
            cssData.AddAttribute(Selector, "font-family", fontFamily);
        }

        public string CreateFontFamilyValue()
        {
            List<string> result = new List<string>(4);
            for(int i = 0; i < 4; i++)
            {
                string fontFace = GetFontAt(i);
                if (fontFace != null &&
                    !result.Contains(fontFace))
                {
                    result.Add(fontFace);
                }
            }
            return String.Join(", ", result);
        }

        private string GetFontAt(int i)
        {
            if (_inlineFonts[i] != null)
                return _inlineFonts[i];
            return _themeFonts[i];
        }

        public override void Extends(CssProperty<RunFonts> other)
        {
            var parent = other as RunFontsCssProperty;
            if (parent == null) return;

            for (int i = 0; i < 4; i++)
            {
                var fontAt = GetFontAt(i);
                if (fontAt != null) continue;
                if (_inlineFonts[i] == null) _inlineFonts[i] = parent._inlineFonts[i];
                if (_themeFonts[i] == null) _themeFonts[i] = parent._themeFonts[i];
            }
        }

        public override short GetSpecificHashcode()
        {
            for (int i = 0; i < 4; i++)
            {
                string fontFace = GetFontAt(i);
                if (fontFace != null) return (short)fontFace.GetHashCode();
            }
            return -1;
        }

        public override bool HaveSameOutput(ICssProperty element)
        {
            var other = element as RunFontsCssProperty;
            if (other == null) return false;

            for(int i = 0; i < 4; i ++)
            {
                if (GetFontAt(i) != other.GetFontAt(i)) return false;
            }

            return true;
        }

        public override ICssProperty Clone()
        {
            var clone = new RunFontsCssProperty();
            clone.Selector = Selector;
            clone.SneakySetElement(Element); 
            _inlineFonts.CopyTo(clone._inlineFonts, 0);
            _themeFonts.CopyTo(clone._themeFonts, 0);
            clone._themeFontProvider = _themeFontProvider;
            return clone;
        }

    }
}
