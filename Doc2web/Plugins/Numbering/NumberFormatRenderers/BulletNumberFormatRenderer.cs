using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Plugins.Numbering.NumberFormatRenderers
{
    public class BulletNumberFormatRenderer : INumberFormatRenderer
    {
        public string Render(int value) => "•";
    }
}
