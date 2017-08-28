using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Properties
{
    public abstract class BaseCssProperty<T> : ICssProperty
        where T : OpenXmlElement
    {

        public string Selector { get; set; }

        public OpenXmlElement OpenXmlElement { get; set; }

        public T Element
        {
            get => (T)OpenXmlElement;
            set => OpenXmlElement = value;
        }

        public abstract CssData AsCss();

        public abstract int CompareTo(T element);

        public int CompareTo(object other)
        {
            if (other is BaseCssProperty<T> otherProp)
                return CompareTo(otherProp.Element);
            return -1;
        }
    }
}
