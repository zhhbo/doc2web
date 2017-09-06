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

        IEnumerable<Mutation> Mutations { get; }

        void ProcessChilden();

        void AddNode(HtmlNode node);

        void AddMultipleNodes(IEnumerable<HtmlNode> nodes);

        void AddMutation(Mutation mutation);

        void AddMutations(IEnumerable<Mutation> mutations);
    }
}
