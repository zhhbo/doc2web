using Doc2web.Plugins.TextFixes;
using Doc2web.Plugins.Style;
using Doc2web.Plugins.TextProcessor;
using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Text;
using Doc2web.Plugins.Numbering;

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
        public static string ConvertCompleteDocument(WordprocessingDocument wpDoc)
        {
            using (var conversionEngine = new ConversionEngine(
                new StylePlugin(wpDoc),
                new NumberingPlugin(wpDoc),
                new TextProcessorPlugin(),
                new CrossReferencesCleanupPlugin(),
                new HyphenInsertionPlugin(),
                new BreakInsertionPlugin(),
                new EscapeHtmlPlugin()
                ))
            {
                return conversionEngine.Convert(wpDoc.MainDocumentPart.Document.Body.Elements());
            }
        }
    }
}
