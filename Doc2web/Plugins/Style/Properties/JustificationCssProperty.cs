using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Properties
{
    [ParagraphCssProperty]
    public class JustificationCssProperty : CssProperty<Justification>
    {
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
            if (i != -1)
                cssData.AddAttribute(Selector, "text-align", MapJusitfyValues[i]);
        }

        public override short GetSpecificHashcode() => JustifyContent;

        public override bool HaveSameOuput(Justification element) =>
            JustifyContent == ConvertElementVal(element);
        

        private bool HasValue => Element.Val?.Value == null;

        private short JustifyContent => ConvertElementVal(Element);

        private static short ConvertElementVal(Justification elem)
        {
            if (elem.Val?.Value == null) return -1;
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
            return -1;
        }

    }
}
