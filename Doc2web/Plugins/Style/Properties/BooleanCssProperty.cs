﻿using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Doc2web.Plugins.Style.Properties
{
    public abstract class BooleanCssProperty<T> : BaseCssProperty<T>
        where T : OnOffType
    {
        protected bool? ExplicitVal => Convert(Element);

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


        public override CssData AsCss()
        {
            var data = new CssData();
            if (ExplicitVal.HasValue)
                if (ExplicitVal.Value) SetOn(data);
                else SetOff(data);
            else SetDefault(data);
            return data;
        }

        protected override short GetSpecificHashcode()
        {
            if (ExplicitVal.HasValue)
            {
                if (ExplicitVal.Value) return 2;
                else return 1;
            }
            return 0;
        }
        public override bool HaveSameOuput(T element) => Convert(element) == ExplicitVal;

    }
}
