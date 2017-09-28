using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public enum CssPropertySource
    {
        Paragraph,
        Run,
        Numbering
    }

    public abstract class BaseCssPropertyAttribute : Attribute
    {
        public BaseCssPropertyAttribute(CssPropertySource source, Type targetedType)
        {
            Source = source;
            TargetedType = targetedType;
        }

        public CssPropertySource Source { get; set; }

        public Type TargetedType { get; set; }

    }

    public class ParagraphCssPropertyAttribute : BaseCssPropertyAttribute
    {
        public ParagraphCssPropertyAttribute(Type targetedType) :
            base(CssPropertySource.Paragraph, targetedType)
        { }
    }

    public class RunCssPropertyAttribute : BaseCssPropertyAttribute
    {
        public RunCssPropertyAttribute(Type targetedType) :
            base(CssPropertySource.Run, targetedType)
        { }
    }

    public class NumberingCssPropertyAttribute : BaseCssPropertyAttribute
    {
        public NumberingCssPropertyAttribute(Type targetedType) :
            base(CssPropertySource.Numbering, targetedType)
        { }
    }
}
