using Doc2web.Plugins.Numbering;
using Doc2web.Plugins.Style;
using Doc2web.Plugins.Style.Properties;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Numbering
{
    [NumberingCssProperty]
    public class NumberingIndentationCssProperty : IdentationCssProperty
    {
        private static string PageMaxMediaQuery = @"(min-width: 21.59cm)";

        private NumberingConfig _numConfig;

        public NumberingIndentationCssProperty(
            StyleConfig config, 
            NumberingConfig numConfig) : base(config)
        {
            _numConfig = numConfig;
        }

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
            //cssData.AddAttribute(ContainerMinSelector, "display", "flex");
            cssData.AddAttribute(ContainerMinSelector, "flex-direction", "row-reverse");

            cssData.AddAttribute(PageMaxMediaQuery, ContainerMaxSelector, "width", ContainerMaxWidthCM);
                
            if (!Hanging.HasValue) return;

            cssData.AddAttribute(NumberMaxSelector, "min-width", NumberMaxWidthVW);
            cssData.AddAttribute(NumberMinSelector, "width", "auto");
            cssData.AddAttribute(PageMaxMediaQuery, NumberMaxSelector, "width", NumberMaxWidthCM);
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
