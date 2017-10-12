using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml;

namespace Doc2web.Plugins.Style.Properties
{
    [ParagraphCssProperty(typeof(Indentation))]
    public class IndentationCssProperty : CssProperty<Indentation>
    {
        private StyleConfig _config;

        public IndentationCssProperty(StyleConfig config)
        {
            _config = config;
        }

        public override OpenXmlElement OpenXmlElement
        {
            get => base.OpenXmlElement;
            set
            {
                base.OpenXmlElement = value;
                ExtractValues();
            }
        }

        #region Extracted props

        public double? Left { get; private set; }

        public double? Right { get; private set; }

        public double? FirstLine { get; private set; }

        public double? Hanging { get; private set; }

        #endregion

        #region CSS values

        public string NoNumberingLeftPadding => ToViewWidth(Left);

        public string RightPadding => ToViewWidth(Right);

        public string NoNumberingTextIndent
        {
            get
            {
                if (Hanging.HasValue)
                    return ToViewWidth(Hanging.Value * -1);
                else if (FirstLine.HasValue)
                    return ToViewWidth(FirstLine.Value);
                return null;
            }
        }

        public string NumberingContainerWith
        {
            get
            {
                if (FirstLine.HasValue && !Hanging.HasValue)
                    return ToViewWidth(Left + FirstLine);
                return ToViewWidth(Left);
            }
        }

        public string NumberingNumberWidth
        {
            get
            {
                if (Hanging.HasValue && Hanging.Value > 0) return ToViewWidth(Hanging);
                if (FirstLine.HasValue && FirstLine.Value > 0) return ToViewWidth(FirstLine);
                return "unset";
            }
        }

        #endregion

        private string ToViewWidth(double v) => Math.Round(v*100, 2) + "vw";

        private string ToViewWidth(double? v) => (v.HasValue) ? ToViewWidth(v.Value) : "0vw";

        private void ExtractValues()
        {
            Left = ExtractValue(Element.Left);
            Right = ExtractValue(Element.Right);
            FirstLine = ExtractValue(Element.FirstLine);
            Hanging = ExtractValue(Element.Hanging);
        }

        private double? ExtractValue(StringValue value)
        {
            if (value?.Value != null)
                return Utils.TwipsToPageRatio(int.Parse(value));
            return null;
        }

        public override int GetHashCode()
        {
            try
            {
                uint f = 0;
                uint s = 0;
                if (Left.HasValue) f = Convert.ToUInt32(Math.Round(Left.Value * 100000, 0) );
                if (Hanging.HasValue) s = Convert.ToUInt32(Math.Round(Hanging.Value * 100000, 0));
                return (int)((f + UInt32.MaxValue/2) | s);
            } catch
            {
                return -1;
            }
        }

        public override bool Equals(ICssProperty obj)
        {
            var other = obj as IndentationCssProperty;
            return
                other != null &&
                other.Left == Left &&
                other.Right == Right &&
                other.FirstLine == FirstLine &&
                other.Hanging == Hanging;
        }

        public override ICssProperty Clone()
        {
            var other = new IndentationCssProperty(_config);
            other.Selector = Selector;
            other.SneakySetElement(Element);
            other.Left = Left;
            other.Right = Right;
            other.FirstLine = FirstLine;
            other.Hanging = Hanging;
            return other;
        }

        public override void Extends(CssProperty<Indentation> parent)
        {
            var other = parent as IndentationCssProperty;
            if (other == null) return;

            if (!Left.HasValue) Left = other.Left;
            if (!Right.HasValue) Right = other.Right;
            if (!FirstLine.HasValue) FirstLine = other.FirstLine;
            if (!Hanging.HasValue) Hanging = other.Hanging;
        }

        private void SneakySetElement(Indentation element)
        {
            base.OpenXmlElement = element;
        }

        public override void InsertCss(CssData cssData)
        {
            if (Left.HasValue)
                InsertLeftPadding(cssData);
            if (Right.HasValue)
                cssData.AddAttribute(Selector, "padding-right", RightPadding);
            if (NoNumberingTextIndent != null)
                cssData.AddAttribute(ContainerWithoutNumbering, "text-indent", NoNumberingTextIndent);
        }

        private void InsertLeftPadding(CssData cssData)
        {
            cssData.AddAttribute(ContainerWithoutNumbering, "padding-left", NoNumberingLeftPadding);
            cssData.AddAttribute(NumberingContainerSelector, "min-width", NumberingContainerWith);
            if (Hanging.HasValue || FirstLine.HasValue)
                cssData.AddAttribute(NumberingNumberSelector, "max-width", NumberingNumberWidth);
        }

        private string ContainerWithoutNumbering => 
            $"{Selector}" +
            $":not({_config.ContainerWithNumberingCssClassSuffix})";

        private string ContainerWithNumbering =>
            $"{Selector}{_config.ContainerWithNumberingCssClassSuffix}";

        private string NumberingContainerSelector =>
            $"{ContainerWithNumbering} {_config.LeftIdentationCssClassPrefix}";

        private string NumberingNumberSelector =>
            $"{ContainerWithNumbering} {_config.NumberingNumber}";
    }
}
