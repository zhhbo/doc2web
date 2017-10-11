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
    public static class QuickAndEasy
    {
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
                return conversionEngine.Render(wpDoc.MainDocumentPart.Document.Body.Elements());
            }
        }
    }
}
