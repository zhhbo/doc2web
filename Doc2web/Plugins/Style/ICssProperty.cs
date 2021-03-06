﻿using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style
{

    public interface ICssProperty
    {
        string Selector { get; set; }

        OpenXmlElement OpenXmlElement { get; set; }

        void Extends(ICssProperty parent);

        void InsertCss(CssPropertiesSet others, CssData cssData);

        ICssProperty Clone();
    }
}
