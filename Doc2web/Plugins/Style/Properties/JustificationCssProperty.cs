using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Properties
{
    [ParagraphCssProperty(typeof(Justification))]
    public class JustificationCssProperty : CssProperty<Justification>
    {
        const byte EMPTY = byte.MaxValue;

        private static string[] MapJusitfyValues = new string[]
        {
            "left",
            "right",
            "center",
            "justify"
        };

        public override void InsertCss(CssData cssData)
        {
            short i = JustifyContent;
            if (i != EMPTY)
                cssData.AddAttribute(Selector, "text-align", MapJusitfyValues[i]);
        }

        public override int GetHashCode() => JustifyContent;

        public override bool Equals(ICssProperty element)
        {
            if (element is JustificationCssProperty other)
            {
                return other.JustifyContent == JustifyContent;
            }
            return false;
        }

        private bool HasValue => Element.Val?.Value == null;

        private byte JustifyContent => ConvertElementVal(Element);

        private static byte ConvertElementVal(Justification elem)
        {
            if (elem.Val?.Value == null) return EMPTY;
            switch (elem.Val.Value)
            {
                case JustificationValues.Start:
                case JustificationValues.Left: return 0;
                case JustificationValues.End:
                case JustificationValues.Right: return 1;
                case JustificationValues.Center: return 2;
                case JustificationValues.Both:
                case JustificationValues.Distribute: return 3;
            }
            return EMPTY;
        }

    }
}
