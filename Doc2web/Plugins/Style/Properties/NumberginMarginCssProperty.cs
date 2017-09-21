using Doc2web.Plugins.Numbering;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Properties
{
    [NumberingCssProperty]
    public class NumberingMarginCssProperty : IdentationCssProperty
    {
        private NumberingProcessorPluginConfig _numConfig;

        public NumberingMarginCssProperty(
            StyleConfiguration config, 
            NumberingProcessorPluginConfig numConfig) : base(config)
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

        protected string ContainerSelector => Selector + "." + _numConfig.NumberingContainerCls;

        protected string NumberSelector =>
            String.Join(" .", Selector, _numConfig.NumberingNumberCls).TrimStart();

        private void SetupLeftKindIdentation(CssData cssData)
        {
            cssData.AddAttribute(ContainerSelector, "display", "flex");
            cssData.AddAttribute(ContainerSelector, "flex-direction", "row-reverse");
            cssData.AddAttribute(
                ContainerSelector,
                "width",
                Math.Round(Utils.TwipsToPageRatio(LeftIndent.Value) * 100, 3) + "vw");
            cssData.AddAttribute(
                "(min-width: 21.59cm)",
                ContainerSelector,
                "width",
                Math.Round(Utils.TwipsToCm(LeftIndent.Value), 3) + "cm");

            if (!Hanging.HasValue) return;

            cssData.AddAttribute(
                NumberSelector,
                "width",
                Math.Round(Utils.TwipsToPageRatio(Hanging.Value) * -100, 3) + "vw");

            cssData.AddAttribute(
                "(min-width: 21.59cm)",
                NumberSelector,
                "width",
                Math.Round(Utils.TwipsToCm(Hanging.Value) * -1, 3) + "cm");
        }
    }
}
