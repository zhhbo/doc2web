using System;
using System.Collections.Generic;
using System.Text;

namespace Whusion
{
    public interface IElementContext
    {
        void AddNode(HtmlNode node);
        void AddTranformation(ITextTransformation transformation);
    }
}
