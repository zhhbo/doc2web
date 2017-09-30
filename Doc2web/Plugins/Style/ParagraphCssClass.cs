using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public class ParagraphCssClass : ICssClass
    {
        public ParagraphCssClass()
        {
            ParagraphProps = new CssPropertiesSet();
            RunProps = new CssPropertiesSet();
        }

        public string Selector
        {
            get => ParagraphProps.Selector;
            set
            {
                ParagraphProps.Selector = value;
                RunProps.Selector = value + " span";
            }
        }

        public ParagraphCssClass BasedOn { get; set; }

        public ParagraphCssClass Defaults { get; set; }

        public CssPropertiesSet ParagraphProps { get; private set; }

        public CssPropertiesSet RunProps { get; private set; }


        public void InsertCss(CssData cssData)
        {
            var pCssProps = new CssPropertiesSet { Selector = ParagraphProps.Selector };
            var rCssProps = new CssPropertiesSet { Selector = ParagraphProps.Selector };

            pCssProps.AddMany(ParagraphProps);
            rCssProps.AddMany(RunProps);

            AddInheritance(pCssProps, rCssProps);
            AddDefaults(pCssProps, rCssProps);

            pCssProps.InsertCss(cssData);
            rCssProps.InsertCss(cssData);
        }

        private void AddDefaults(CssPropertiesSet pCssProps, CssPropertiesSet rCssProps)
        {
            if (Defaults != null)
            {
                pCssProps.AddMany(Defaults.ParagraphProps);
                rCssProps.AddMany(Defaults.RunProps);
            }
        }

        private void AddInheritance(CssPropertiesSet pCssProps, CssPropertiesSet rCssProps)
        {
            var basedOn = BasedOn;
            while (basedOn != null)
            {
                pCssProps.AddMany(basedOn.ParagraphProps);
                rCssProps.AddMany(basedOn.RunProps);
                basedOn = basedOn.BasedOn;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is ParagraphCssClass other)
                return ParagraphProps.SetEquals(other.ParagraphProps) &&
                       RunProps.SetEquals(other.RunProps);
            return false;
        }

        public override int GetHashCode()
        {
            return (BasedOn?.GetHashCode(), ParagraphProps.GetHashCode(), RunProps.GetHashCode()).GetHashCode();
        }

        public bool IsEmpty => ParagraphProps.Count == 0 && RunProps.Count == 0;
    }
}
