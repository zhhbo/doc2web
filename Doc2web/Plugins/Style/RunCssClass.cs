using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public class RunCssClass
    {
        public RunCssClass()
        {
            RunProps = new CssPropertiesSet();
        }

        public string Selector
        {
            get => RunProps.Selector;
            set => RunProps.Selector = value;
        }

        public RunCssClass BasedOn { get; set; }

        public CssPropertiesSet RunProps { get; private set; }

        public CssData AsCss()
        {
            var cssPropertySet = new CssPropertiesSet();

            foreach (var prop in RunProps)
                cssPropertySet.Add(prop);

            if (BasedOn != null)
                foreach (var prop in BasedOn.RunProps)
                    cssPropertySet.Add(prop);

            cssPropertySet.Selector = Selector;
            return cssPropertySet.AsCss();
        }
    }
}
