using Autofac;
using Doc2web.Core;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web
{
    /// <summary>
    /// Context that represent the current state of all root elements.
    /// </summary>
    public interface IGlobalContext
    {
        /// <summary>
        /// All IElementContext for each root elements passed to the ConversionEngine.
        /// </summary>
        IEnumerable<RootElementContext> RootElements { get; }

        /// <summary>
        /// Html that will be inserted after the conversion result of the RootElements.
        /// </summary>
        string Html { get; }

        /// <summary>
        /// CSS that will be inserted in the heading of the Html document.
        /// </summary>
        string Css { get; }

        /// <summary>
        /// Javascript that will be inserted at the end of the html document.
        /// </summary>
        string Js { get; }

        /// <summary>
        /// Add some html the HTML property.
        /// </summary>
        /// <param name="html"></param>
        void AddHtml(string html);

        /// <summary>
        /// Add some css to the CSS property.
        /// </summary>
        /// <param name="css"></param>
        void AddCss(string css);

        /// <summary>
        /// Add soome javascript to the JS property.
        /// </summary>
        /// <param name="js"></param>
        void AddJs(string js);

        /// <summary>
        /// Resolve a service using the IoC container.
        /// </summary>
        /// <typeparam name="T">Type of the service to be resolved.</typeparam>
        /// <returns>Service instance that has been resolved.</returns>
        T Resolve<T>();

        /// <summary>
        /// Try to resolve a service using the IoC container.
        /// </summary>
        /// <typeparam name="T">Type of service to be resolved.</typeparam>
        /// <param name="service">Service instance to be resolved.</param>
        /// <returns>Returns TRUE if the service could be resolved.</returns>
        bool TryResolve<T>(out T service);
    }
}
