using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Properties
{
    public abstract class CssProperty<T> : ICssProperty
        where T : OpenXmlElement
    {
        public string Selector { get; set; }

        public virtual OpenXmlElement OpenXmlElement { get; set; }

        public virtual T Element
        {
            get => (T)OpenXmlElement;
            set => OpenXmlElement = value;
        }

        public virtual void InsertCss(CssData cssData) { }

        public virtual void InsertCss(CssPropertiesSet others, CssData cssData)
        {
            InsertCss(cssData);
        }

        public abstract override int GetHashCode();

        public abstract bool Equals(ICssProperty obj);

        public override bool Equals(object obj)
        {
            if (obj is ICssProperty other) return Equals(other);
            return base.Equals(obj);
        }

        public virtual void Extends(CssProperty<T> parent) { }

        public override string ToString()
        {
            var data = new CssData();
            InsertCss(data);
            return data.ToString();
        }

        public void Extends(ICssProperty parent)
        {
            if (parent is CssProperty<T> casted)
            {
                Extends(casted);
            }
        }

        public virtual ICssProperty Clone()
        {
            var clone = GetType().GetConstructors()[0].Invoke(new object[] { }) as ICssProperty;
            clone.OpenXmlElement = OpenXmlElement;
            clone.Selector = Selector;
            return clone;
        }
    }
}
