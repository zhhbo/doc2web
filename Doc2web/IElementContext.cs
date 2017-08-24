using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web
{
    public interface IElementContext
    {
        IGlobalContext GlobalContext { get; }

        OpenXmlElement RootElement { get; }

        OpenXmlElement Element { get; }

        int TextIndex { get; }
        
        IEnumerable<HtmlNode> Nodes { get; }

        IEnumerable<ITextTransformation> Transformations { get; }

        void ProcessChilden();

        void AddNode(HtmlNode node);

        void AddMultipleNodes(IEnumerable<HtmlNode> nodes);

        void AddTranformation(ITextTransformation transformation);

        void AddMultipleTransformations(IEnumerable<ITextTransformation> transformations);
    }
}
