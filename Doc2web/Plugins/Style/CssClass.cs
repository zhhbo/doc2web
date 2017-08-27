using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public class CssClass
    {
        public string Name { get; set; }

        public string InheritsFrom { get; set; }

        public List<ICssProperty> Properties { get; }

        public string Render()
        {
            throw new NotImplementedException();
        }
    }
}
