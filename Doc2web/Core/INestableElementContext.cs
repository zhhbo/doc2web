using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Core
{
    public interface INestableElementContext : IElementContext
    {
        IContextNestingHandler NestingHandler { get; set; }
    }
}
