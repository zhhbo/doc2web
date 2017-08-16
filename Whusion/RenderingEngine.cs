using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Whusion
{
    public class RenderingEngine : IDisposable
    {
        public RenderingEngine(params object[] plugins)
        {
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public string Render(IEnumerable<OpenXmlElement> elements)
        {
            throw new NotImplementedException();
        }
    }
}
