using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Properties
{
    public static class TypeExtension
    {
        public static Type AsRegistrableCssProperty(this Type x)
        {
            var t = typeof(CssProperty<>);
            while(x.GetGenericArguments().Length != 1)
            {
                if (x.BaseType == typeof(object))
                    throw new InvalidProgramException("A classs ending in CssProperty is invalid");
                x = x.BaseType;
            }
            return t.MakeGenericType(x.GetGenericArguments());
        }
    }

    public static class Utils
    {
        public static CssData AsCss(this ICssProperty prop)
        {
            var cssData = new CssData();
            prop.InsertCss(new CssPropertiesSet(), cssData);
            return cssData;
        }

        public static double TwipsToCm(int twips)
        {
            return twips / 567.0;
        }

        public static double TwipsToPageRatio(int twips)
        {
            return twips / (567.0 * 21.59);
        }

        public static double? GetSize(HpsMeasureType element)
        {
            var val = element.Val?.Value;
            if (val != null && int.TryParse(val, out int result))
                return result / 2;
            return null;
        }

        public static bool ComplexScriptApplies(CssPropertiesSet set)
        {
            var csProp = set.Get<ComplexScriptCssProperty>();
            if (csProp != null && csProp.ExplicitVal.GetValueOrDefault(true))
                return true;

            var fonts = set.Get<RunFontsCssProperty>();
            if (fonts != null && fonts.CanOnlyUseComplexScript)
                return true;

            return false;
        }
    }

    public class IndentationDecorator
    {
        public Indentation Element {get; set;}

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
    }
}
