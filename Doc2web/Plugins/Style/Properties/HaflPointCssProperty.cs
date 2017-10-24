using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml;

namespace Doc2web.Plugins.Style.Properties
{
    public class HaflPointCssProperty<T> : CssProperty<T> where T : HpsMeasureType
    {
        private double _val = -1;
        protected double Val => _val;

        public override OpenXmlElement OpenXmlElement
        {
            get => base.OpenXmlElement;
            set
            {
                base.OpenXmlElement = value;
                ExtractValue();
            }
        }

        public double Size => Math.Round(_val, 2);

        private void ExtractValue()
        {
            _val = Utils.GetSize(Element).GetValueOrDefault(-1);

        }

        public override bool Equals(ICssProperty obj)
        {
            if (obj is HaflPointCssProperty<T> other) {
                return other._val == _val;
            }
            return false;
        }

        public override int GetHashCode() => (int)(_val * 100);
    }
}
