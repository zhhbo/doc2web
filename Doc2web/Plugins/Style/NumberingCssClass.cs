using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public class NumberingCssClass : ICssClass
    {
        public CssPropertiesSet ContainerProps { get; set; }

        public CssPropertiesSet NumberProps { get; set; }

        public NumberingCssClass()
        {
            ContainerProps = new CssPropertiesSet();
            NumberProps = new CssPropertiesSet();
        }

        public string Selector
        {
            get => $".numbering-{NumberingId}-{Level}";
            set => 
                throw new InvalidOperationException(
                    "Cannot manually set the selector of a numbering css class");
        }

        public int NumberingId { get; set; }

        public int Level { get; set; }

        public bool IsEmpty => ContainerProps.Count + NumberProps.Count == 0;

        public void InsertCss(CssData cssData)
        {
            ContainerProps.Selector = Selector;
            NumberProps.Selector = $"{Selector} .nubmering-number";
            ContainerProps.InsertCss(cssData);
            NumberProps.InsertCss(cssData);
        }
    }
}
