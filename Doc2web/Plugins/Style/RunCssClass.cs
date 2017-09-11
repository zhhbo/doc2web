using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public class RunCssClass : ICssClass
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

        public void InsertCss(CssData cssData)
        {
            var cssPropertySet = new CssPropertiesSet { Selector = Selector };

            var target = this;
            while (target != null)
            {
                cssPropertySet.AddMany(target.RunProps);
                target = target.BasedOn;
            }

            cssPropertySet.InsertCss(cssData);
        }

        public override bool Equals(object obj)
        {
            if (obj is RunCssClass other)
                return RunProps.SetEquals(other.RunProps);
            return false;
        }

        public override int GetHashCode()
        {
            return (BasedOn?.GetHashCode(), RunProps.GetHashCode()).GetHashCode();
        }

        public bool IsEmpty => RunProps.Count == 0;
    }
}
