using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Plugins.Numbering
{
    public class LinkedStyleNumberingException : Exception
    {
        public LinkedStyleNumberingException(string linkedStyleId)
          : base($"This numbering is linked to a style #{linkedStyleId}, please use the numbering on this style instead")
        {
            Data["linkedStyleId"] = linkedStyleId;
        }

        public string LinkedStyleId => Data["linkedStyleId"].ToString();
    }
}
