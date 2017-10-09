using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml;

namespace Doc2web.Plugins.Style.Properties
{
    [ParagraphCssProperty(typeof(SpacingBetweenLines))]
    public class SpacingCssProperty : CssProperty<SpacingBetweenLines>
    {

        private double? _after;
        private double? _before;
        private double? _line;

        public override OpenXmlElement OpenXmlElement
        {
            get => base.OpenXmlElement;
            set
            {
                base.OpenXmlElement = value;
                InitSpacing();
            }

        }

        public double? After => _after;
        public double? Before => _before;
        public double? Line => _line;

        private void InitSpacing()
        {
            _before =
                IsNullOrOn(Element.BeforeAutoSpacing) ?
                null : TryParse(Element.Before?.Value);
            _after = 
                IsNullOrOn(Element.AfterAutoSpacing) ? 
                null : TryParse(Element.After?.Value);
            _line = TryParse(Element.Line?.Value);
        }

        private bool IsNullOrOn(OnOffValue v)
        {
            if (v == null) return false;

            if (!v.HasValue || v.Value) return true;

            return false;
        }

        private double? TryParse(string value)
        {
            if (value == null) return null;
            if (int.TryParse(value, out int result))
                return result / 20;
            return null;
        }

        public override short GetSpecificHashcode()
        {
            return (short)(_after, _before, _line).GetHashCode();
        }

        public override bool HaveSameOutput(ICssProperty prop)
        {
            if (prop is SpacingCssProperty other)
            {
                return After == other.After && Before == other.Before && Line == other.Line;
            }
            return false;
        }

        public override void InsertCss(CssData cssData)
        {
            if (Line.HasValue)
            {
                cssData.AddAttribute(Selector, "line-height", Math.Round(Line.Value, 4) + "pt");
            }
        }

        public override void Extends(CssProperty<SpacingBetweenLines> parent)
        {
            if (parent is SpacingCssProperty other)
            {
                if (_after == null) _after = other._after;
                if (_before == null) _before = other._before;
                if (_line == null) _line = other._line;
            }
        }
    }
}
