﻿using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style.Properties
{
    public class HighlightCssProperty : CssProperty<Highlight>
    {
        public override CssData AsCss()
        {
            var data = new CssData();
            var i = GetColorCode(Element);
            if (i > -1)
                data.AddAttribute(Selector, "background-color", ColorAssociation[i]);
            return data;
        }

        public override short GetSpecificHashcode() => GetColorCode(Element);

        public override bool HaveSameOuput(Highlight element) =>
            GetColorCode(element) == GetSpecificHashcode();

        private static string[] ColorAssociation = new string[]
        {
            "#000000",
            "#0000FF",
            "#00FFFF",
            "#008000",
            "#FF00FF",
            "#FF0000",
            "#FFFF00",
            "#FFFFFF",
            "#00008B",
            "#008B8B",
            "#006400",
            "#8B008B",
            "#8B0000",
            "#8B8B00",
            "#A9A9A9",
            "#D3D3D3",
            ""
        };

        private static short GetColorCode(Highlight elem)
        {
            if (elem?.Val?.Value == null) return -1;
            return (short)elem.Val.Value;
        }
    }
}