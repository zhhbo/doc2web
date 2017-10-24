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

        public bool CanOnlyUseComplexScript =>
            _inlineFonts[0] == null &&
            _inlineFonts[1] == null &&
            _inlineFonts[2] == null &&
            _themeFonts[0] == null &&
            _themeFonts[1] == null &&
            _themeFonts[2] == null &&
            (_inlineFonts[3] != null || _themeFonts[3] != null);

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

        /// <summary>
        /// Only as a convience in unit tests.
        /// </summary>
        public override void InsertCss(CssData cssData)
        {
            InsertCss(new CssPropertiesSet(), cssData);
        }

        public override void InsertCss(CssPropertiesSet set, CssData cssData)
        {
            var complexScript = set?.Get<ComplexScriptCssProperty>();
            if (complexScript != null)
                UseComplexScript(complexScript, cssData);
            else
                UseAllFonts(cssData);
        }

        private void UseComplexScript(ComplexScriptCssProperty prop, CssData cssData)
        {
            if (!prop.ExplicitVal.GetValueOrDefault(true)) return;
            string fontFamily = GetFontAt(3);
            if (fontFamily == null) return;
            cssData.AddAttribute(Selector, "font-family", fontFamily);
        }

        private void UseAllFonts(CssData cssData)
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
                    fontFace.Length > 0 &&
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

        public override int GetHashCode()
        {
            for (int i = 0; i < 4; i++)
            {
                string fontFace = GetFontAt(i);
                if (fontFace != null) return fontFace.GetHashCode();
            }
            return -1;
        }

        public override bool Equals(ICssProperty element)
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
