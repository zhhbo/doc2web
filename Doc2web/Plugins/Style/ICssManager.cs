using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public interface ICssManager
    {
        /// <summary>
        /// Find the style with the matching style id and create a css class from it.
        /// </summary>
        /// <param name="styleId">Targeted style id.</param>
        /// <returns>Css class representing this style id.</returns>
        string RegisterStyleIdAsClass(string styleId);

        /// <summary>
        /// Create a css class from paragraph properties.
        /// </summary>
        /// <param name="pPr">Paragraph properties used to create the css class.</param>
        /// <returns>Css class created from the paragraph properties.</returns>
        string RegisterPropertiesAsClass(ParagraphProperties pPr);

        /// <summary>
        /// Create a css class from run properties.
        /// </summary>
        /// <param name="rPr">Run properties used to create the css class.</param>
        /// <returns>Css class created from the run properties.</returns>
        string RegisterPropertiesAsClass(RunProperties rPr);
    }
}
