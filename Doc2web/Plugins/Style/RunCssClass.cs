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

        public RunCssClass Defaults { get; set; }

        public RunCssClass BasedOn { get; set; }

        public CssPropertiesSet RunProps { get; private set; }

        public void InsertCss(CssData cssData)
        {
            var cssPropertySet = new CssPropertiesSet { Selector = Selector };

            AddInheritance(cssPropertySet);
            AddDefaults(cssPropertySet);

            cssPropertySet.InsertCss(cssData);
        }

        private void AddDefaults(CssPropertiesSet cssPropertySet)
        {
            if (Defaults != null)
                cssPropertySet.AddMany(Defaults.RunProps);
        }

        private void AddInheritance(CssPropertiesSet cssPropertySet)
        {
            var target = this;
            while (target != null)
            {
                cssPropertySet.AddMany(target.RunProps);
                target = target.BasedOn;
            }
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
