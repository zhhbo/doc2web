using System;
using System.Collections.Generic;

namespace Whusion
{
    public class HtmlNode
    {
        public string Tag { get; set; }
        public int Z { get; set; }
        public IReadOnlyCollection<string> Classes { get; }
        public IReadOnlyDictionary<string, string> Attributes { get; }
        public IReadOnlyDictionary<string, string> Style { get; }

        public void AddClass(string name)
        {

        }

        public void SetStyle(string name, string value)
        {

        }

        public void AddAttribute(string name, string value)
        {

        }
        
    }
}
