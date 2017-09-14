using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml;

namespace Doc2web.Plugins.Style.Properties
{
    public class IdentationCssProperty : CssProperty<Indentation>
    {
        private StyleConfiguration _config;

        public IdentationCssProperty(StyleConfiguration config) : base()
        {
            _config = config;
        }

        public override void InsertCss(CssData cssData)
        {
            AddTryIdentation(cssData, LeftIndent, _config.LeftIdentationCssClassPrefix);
            AddTryIdentation(cssData, RightIndent, _config.RightIdentationCssClassPrefix);
        }

        private void AddTryIdentation(CssData cssData, int? indent, string cssSuffix)
        {
            if (!indent.HasValue) return;
            string selector = $"{Selector} {cssSuffix}";
            cssData.AddAttribute(
                selector,
                "max-width",
                $"{Math.Round(Utils.TwipsToCm(indent.Value), 3)}cm"
            );
            cssData.AddAttribute(
                selector,
                "width",
                $"{Math.Round(Utils.TwipsToPageRatio(indent.Value) * 100, 3)}%"
            );
            cssData.AddAttribute(
                selector,
                "min-width",
                $"fit-content"
            );
        }

        private int? LeftIndent => TryGetValue(Element.Left);

        private int? RightIndent => TryGetValue(Element.Right);

        private int? TryGetValue(StringValue v)
        {
            if (v?.Value != null)
            {
                try
                {
                    return int.Parse(v.Value);
                } catch
                {
                    return null;
                }
            }
            return null;
        }

        public override short GetSpecificHashcode()
        {
            if (LeftIndent.HasValue) return (short)LeftIndent.Value;
            return 0;
        }

        public override bool HaveSameOuput(Indentation element)
        {
            var other = new IdentationCssProperty(_config) { Element = element };
            return other.LeftIndent == LeftIndent && other.RightIndent == RightIndent;
        }

    }
}
