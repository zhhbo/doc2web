using Doc2web.Plugins.TextFixes;
using Doc2web.Plugins.Style;
using Doc2web.Plugins.TextProcessor;
using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Text;
using Doc2web.Plugins.Numbering;
using DocumentFormat.OpenXml;
using Doc2web.Plugins.Tables;
using Doc2web.Plugins.TableOfContent;

namespace Doc2web
{
    /// <summary>
    /// Static class that shows the simplest usage of Doc2web.
    /// </summary>
    public static class QuickAndEasy
    {
        /// <summary>
        /// Convert a completed document using all pre-made plugins.
        /// </summary>
        /// <param name="wpDoc">Document to be converted.</param>
        /// <returns>Html document that is result of the conversion.</returns>
        public static string ConvertCompleteDocument(WordprocessingDocument wpDoc) =>
            ConvertPartialDocument(wpDoc, wpDoc.MainDocumentPart.Document.Body.Elements());

        /// <summary>
        /// Convert partialy a document using all pre-made plugins.
        /// </summary>
        /// <param name="wpDoc">Document to be converted.</param>
        /// <param name="elements">OpenXmlElements to convert. Must be part of the document.</param>
        /// <returns>Html document that is result of the conversion.</returns>
        public static string ConvertPartialDocument(WordprocessingDocument wpDoc, IEnumerable<OpenXmlElement> elements)
        {
            using (var conversionEngine = BuildDefaultEngine(wpDoc))
            {
                return conversionEngine.Convert(elements);
            }
        }

        /// <summary>
        /// Create a conversion engine using all plugins provided in Doc2web.
        /// </summary>
        /// <param name="wpDoc"></param>
        /// <returns></returns>
        public static ConversionEngine BuildDefaultEngine(WordprocessingDocument wpDoc) =>
            new ConversionEngine(
                new StylePlugin(wpDoc),
                new NumberingPlugin(wpDoc),
                new TextProcessorPlugin(),
                new TableOfContentPlugin(),
                new TablePlugin(),
                new CrossReferencesCleanupPlugin(),
                new HyphenInsertionPlugin(),
                new BreakInsertionPlugin(),
                new EscapeHtmlPlugin()
            );
    }
}
