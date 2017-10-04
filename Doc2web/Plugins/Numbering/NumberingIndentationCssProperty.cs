using Doc2web.Plugins.Numbering;
using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Properties;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Numbering
{
    [NumberingCssProperty(typeof(Indentation))]
    public class NumberingIndentationCssProperty : CssProperty<Indentation>
    {
        private static string PageMaxMediaQuery = @"(min-width: 21.59cm)";
        private IdentationCssProperty _indentProp;
        private NumberingConfig _numConfig;

        public NumberingIndentationCssProperty(
            StyleConfig config, 
            NumberingConfig numConfig) : base()
        {
            _indentProp = new IdentationCssProperty(config);
            _numConfig = numConfig;
        }

        private NumberingIndentationCssProperty() { }

        public override OpenXmlElement OpenXmlElement
        {
            get => base.OpenXmlElement;
            set {
                base.OpenXmlElement = value;
                _indentProp.OpenXmlElement = value;
            }
        }

        private int? LeftIndent => _indentProp.LeftIndent;
        private int? RightIndent => _indentProp.RightIndent;
        private int? Hanging => _indentProp.Hanging;


        public override void InsertCss(CssData cssData)
        {
            if (LeftIndent.HasValue && LeftIndent > 0)
            {
                SetupLeftKindIdentation(cssData);
            } else if (RightIndent.HasValue)
            {
            }
        }

        private void SetupLeftKindIdentation(CssData cssData)
        {
            cssData.AddAttribute(ContainerMaxSelector, "min-width", ContainerMaxWidthVW);
            cssData.AddAttribute(ContainerMinSelector, "width", "auto");
            cssData.AddAttribute(ContainerMinSelector, "flex-direction", "row-reverse");
            cssData.AddAttribute(PageMaxMediaQuery, ContainerMaxSelector, "min-width", ContainerMaxWidthCM);
                
            if (!Hanging.HasValue) return;

            cssData.AddAttribute(NumberMaxSelector, "min-width", NumberMaxWidthVW);
            cssData.AddAttribute(NumberMinSelector, "width", "auto");
            cssData.AddAttribute(PageMaxMediaQuery, NumberMaxSelector, "min-width", NumberMaxWidthCM);
        }

        public override short GetSpecificHashcode() => _indentProp.GetSpecificHashcode();


        public override bool HaveSameOutput(ICssProperty prop)
        {
            if (prop is NumberingIndentationCssProperty other)
            {
                return _indentProp.HaveSameOutput(other._indentProp);
            }
            return false;
        }

        public override ICssProperty Clone()
        {
            var clone = new NumberingIndentationCssProperty();
            clone.Selector = Selector;
            clone._numConfig = _numConfig;
            clone._indentProp = _indentProp.Clone() as IdentationCssProperty;
            return clone;
        }

        private string ContainerMaxSelector => 
            Selector + "." + _numConfig.NumberingContainerMaxCls;

        private string ContainerMinSelector => 
            Selector + " ." + _numConfig.NumberingContainerMinCls;
        
        private string NumberMaxSelector => 
            Selector + " ." + _numConfig.NumberingNumberMaxCls;

        private string NumberMinSelector => 
            Selector + " ." + _numConfig.NumberingNumberMinCls;

        private string ContainerMaxWidthVW =>
             Math.Round(Utils.TwipsToPageRatio(LeftIndent.Value) * 100, 3) + "vw";

        private string NumberMaxWidthVW =>
            Math.Round(Utils.TwipsToPageRatio(Hanging.Value) * -100, 3) + "vw";

        private string NumberMaxWidthCM =>
            Math.Round(Utils.TwipsToCm(Hanging.Value) * -1, 3) + "cm";

        private string ContainerMaxWidthCM =>
            Math.Round(Utils.TwipsToCm(LeftIndent.Value), 3) + "cm";
    }
}
