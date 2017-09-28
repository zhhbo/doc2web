using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml;

namespace Doc2web.Plugins.Style.Properties
{
    [ParagraphCssProperty(typeof(Indentation))]
    public class IdentationCssProperty : CssProperty<Indentation>
    {
        protected StyleConfig _config;

        public IdentationCssProperty(StyleConfig config) : base()
        {
            _config = config;
        }

        public override void InsertCss(CssData cssData)
        {
            AddTryIdentation(cssData, LeftIndent, _config.LeftIdentationCssClassPrefix);
            AddTryIdentation(cssData, RightIndent, _config.RightIdentationCssClassPrefix);
            AddTryAlinea(cssData, Hanging);
            AddTryAlinea(cssData, FirstLine);
        }

        private void AddTryAlinea(CssData cssData, int? indentation)
        {
            if (!indentation.HasValue) return;
            cssData.AddAttribute(
                @"(max-width: 21.59cm)",
                Selector,
                "text-indent",
                Math.Round(Utils.TwipsToPageRatio(indentation.Value) * 100, 3) + "vw"
            );
            cssData.AddAttribute(
                @"(min-width: 21.59cm)",
                Selector,
                "text-indent",
                Math.Round(Utils.TwipsToCm(indentation.Value), 3) + "cm"
            );
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
                $"{Math.Round(Utils.TwipsToPageRatio(indent.Value) * 100, 3)}vw"
            );
            cssData.AddAttribute(
                selector,
                "min-width",
                $"fit-content"
            );
        }

        public int? LeftIndent => TryGetValue(Element.Left);

        public int? RightIndent => TryGetValue(Element.Right);

        public int? Hanging
        {
            get
            {
                var x = TryGetValue(Element.Hanging);
                if (x.HasValue) return x * -1;
                return x;

            }
        }
        public int? FirstLine => TryGetValue(Element.FirstLine);

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
