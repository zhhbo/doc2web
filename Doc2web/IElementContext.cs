using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web
{
    /// <summary>
    /// Context that represent the current state of conversion an open xml element.
    /// </summary>
    public interface IElementContext
    {
        /// <summary>
        /// Try to resolve a service throught the IoC container
        /// </summary>
        /// <typeparam name="T">Type of the service.</typeparam>
        /// <returns>Service instance that has been resolve.</returns>
        T Resolve<T>();

        /// <summary>
        /// OpenXmlElement that is the root of this context. Must
        /// has been passed to the ConversionEngine directly.
        /// </summary>
        OpenXmlElement RootElement { get; }

        /// <summary>
        /// Current element that is getting converted. Must
        /// be one of the descendants of the RootElement.
        /// </summary>
        OpenXmlElement Element { get; }

        /// <summary>
        /// Textual index of the Element. This is the position in the 
        /// text of the Element.
        /// </summary>
        int TextIndex { get; }
        
        /// <summary>
        /// HtmlNodes that has been register in this context. This will include HtmlNodes
        /// registered in the previous open xml element and ancestor up to the RootElement.
        /// </summary>
        IEnumerable<HtmlNode> Nodes { get; }

        /// <summary>
        /// Mutatioons that has been register in this context. This will include Mutations
        /// registered in the previous open xml element and ancestor up to the RootElement.
        /// </summary>
        IEnumerable<Mutation> Mutations { get; }

        /// <summary>
        /// Dictionary of objects that are passed as utility.
        /// </summary>
        IDictionary<string, object> ViewBag { get; }

        /// <summary>
        /// Register as tasks to convert all children of the Element. These tasks will share
        /// an element context with the same RootElement, ViewBag, Nodes and Mutations.
        /// </summary>
        void ProcessChilden();

        /// <summary>
        /// Register an HtmlNode to be rendered.
        /// </summary>
        void AddNode(HtmlNode node);

        /// <summary>
        /// Register multiple HtmlNode to be rendered.
        void AddMultipleNodes(IEnumerable<HtmlNode> nodes);

        /// <summary>
        /// Register a mutation that will be applied to the textual output.
        /// </summary>
        void AddMutation(Mutation mutation);

        /// <summary>
        /// Registerr multiple mutations that will be applied to the textual output.
        /// </summary>
        void AddMutations(IEnumerable<Mutation> mutations);
    }
}
