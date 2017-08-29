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
                RunProps.Selector = value = " span";
            }
        }

        public ParagraphCssClass BasedOn { get; set; }

        public CssPropertiesSet ParagraphProps { get; private set; }

        public CssPropertiesSet RunProps { get; private set; }

        public CssData AsCss()
        {
            var cssData = new CssData();
            cssData.AddRange(BuildParagraphCssData());
            cssData.AddRange(BuildRunCssData());
            return cssData;
        }

        private CssData BuildParagraphCssData()
        {
            var cssPropertySet = new CssPropertiesSet();

            foreach (var prop in ParagraphProps)
                cssPropertySet.Add(prop);

            if (BasedOn != null)
                foreach (var prop in BasedOn.ParagraphProps)
                    cssPropertySet.Add(prop);

            cssPropertySet.Selector = Selector;
            return cssPropertySet.AsCss();
        }

        private CssData BuildRunCssData()
        {
            var cssPropertySet = new CssPropertiesSet();

            foreach (var prop in RunProps)
                cssPropertySet.Add(prop);

            if (BasedOn != null)
                foreach (var prop in BasedOn.RunProps)
                    cssPropertySet.Add(prop);

            cssPropertySet.Selector = RunProps.Selector;
            return cssPropertySet.AsCss();
        }
    }
}
