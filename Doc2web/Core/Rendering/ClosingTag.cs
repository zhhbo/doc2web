﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Core.Rendering
{
    public class ClosingTag : ITag
    {
        public OpeningTag Related { get; set; }

        public string Name => Related.Name;
        
        public int Index { get; set; }

        public int RelatedIndex => Related.Index;

        public int Z => Related.Z;

        public string Render() => $"</{Related.Name}>";

    }

}
