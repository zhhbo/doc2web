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

        public override bool Equals(object obj)
        {
            if (obj is BaseCssProperty<T> prop)
                return HaveSameOuput(prop.Element);
            return false;
        }

        public override int GetHashCode()
        {
            var t = GetType().GetHashCode() + short.MaxValue;
            return 0 | t | (int)GetSpecificHashcode();
        }

        public abstract short GetSpecificHashcode();
        public abstract bool HaveSameOuput(T element);
    }
}
