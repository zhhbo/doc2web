using Doc2web.Plugins.Style.Css;
using Doc2web.Plugins.Style.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doc2web.Plugins.TextProcessor
{
    public class MarginApplier
    {
        private TextProcessorConfig _config;
        private IElementContext[] _elements;
        private IElementContext Current => _elements[_i];
        private IElementContext Next => _elements[_i + 1];
        private int _i;

        private bool IsFirst => _i == 0;
        private bool HasNext => _i < _elements.Length - 1;

        public MarginApplier(TextProcessorConfig config, IEnumerable<IElementContext> elements)
        {
            _config = config;
            _elements = elements.ToArray();
        }

        public void Apply()
        {
            for (int i = 0; i < _elements.Length; i++)
            {
                _i = i;
                SetMargins();
            }
        }

        private void SetMargins()
        {
            var currentSpacing = GetSpacingProp(Current);

            if (IsFirst && currentSpacing?.Before != null)
                SetMarginTop(currentSpacing.Before.Value);

            if (!HasNext && currentSpacing?.After != null)
                SetMarginTopBottom(currentSpacing.After.Value);
            else if (HasNext)
                UseLargerSpacing(currentSpacing);
        }

        private void UseLargerSpacing(SpacingCssProperty currentSpacing)
        {
            double? currentAfter = currentSpacing?.After;
            double? nextBefore = GetSpacingProp(Next)?.Before;
            double? spacing = GetBestMarginBottomValue(currentAfter, nextBefore);

            SetMarginTopBottom(spacing);
        }

        private double? GetBestMarginBottomValue(double? current, double? next)
        {
            if (current.HasValue && next.HasValue)
            {
                if (current.Value > next.Value) return current;
                return next;
            }
            else if (current.HasValue) return current;
            else if (next.HasValue) return next;
            return null;
        }

        private void SetMarginTopBottom(double? value)
        {
            if (!value.HasValue) return;
            GetContainerNode()
                .SetStyle("margin-bottom", value + "pt");
        }

        private void SetMarginTop(double? before)
        {
            if (!before.HasValue) return;
            GetContainerNode()
                .SetStyle("margin-top", before + "pt");
        }

        private HtmlNode GetContainerNode()
        {
            return Current.Nodes.FirstOrDefault(x => x.Classes.Contains(_config.ContainerCls));
        }

        private SpacingCssProperty GetSpacingProp(IElementContext context)
        {
            if (context.ViewBag.TryGetValue(_config.PPropsCssClassKey, out object cls))
            {
                var cssClass = cls as CssClass;
                if (cssClass == null) return null;
                return cssClass.Props.Get<SpacingCssProperty>();
            }
            return null;
        }
    }
}
