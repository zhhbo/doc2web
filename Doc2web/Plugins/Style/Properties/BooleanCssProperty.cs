﻿using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Plugins.Style.Properties
{
    public abstract class BooleanCssProperty<T> : CssProperty<T>
        where T : OnOffType
    {
        public override OpenXmlElement OpenXmlElement
        {
            get => base.OpenXmlElement;
            set
            {
                base.OpenXmlElement = value;
                ExplicitVal = Convert(Element);
            }
        }

        public bool? ExplicitVal { get; private set; }

        private static bool? Convert(OnOffType elem)
        {
            if (elem.Val != null && elem.Val.HasValue)
                return elem.Val.Value;
            return null;
        }

        public virtual void SetDefault(CssData data)
        {
            SetOn(data);
        }

        public abstract void SetOn(CssData data);

        public abstract void SetOff(CssData data);


        public override void InsertCss(CssData cssData)
        {
            if (ExplicitVal.HasValue)
                if (ExplicitVal.Value) SetOn(cssData);
                else SetOff(cssData);
            else SetDefault(cssData);
        }

        public override int GetHashCode()
        {
            if (ExplicitVal.HasValue)
            {
                if (ExplicitVal.Value) return 2;
                else return 1;
            }
            return 0;
        }
        public override bool Equals(ICssProperty element)
        {
            if (element is BooleanCssProperty<T> other)
            {
                return other.ExplicitVal == ExplicitVal;
            }
            return false;
        }

        public override void Extends(CssProperty<T> parent)
        {
            var other = parent as BooleanCssProperty<T>;
            if (other == null) return;

            if (!ExplicitVal.HasValue && other.ExplicitVal.HasValue)
                ExplicitVal = other.ExplicitVal;
        }

    }
}
