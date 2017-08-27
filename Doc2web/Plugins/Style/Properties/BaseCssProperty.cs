using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Properties
{
    public abstract class BaseCssProperty<T> : ICssProperty
        where T : OpenXmlElement
    {
        public T Element { get; set; }

        public string Selector { get; set; }

        public abstract CssData AsCss();

        public abstract int CompareTo(T element);

        public int CompareTo(object other)
        {
            var otherProp = other as BaseCssProperty<T>;
            if (otherProp != null) return CompareTo(otherProp.Element);
            return -1;
        }
    }
}
