using Doc2web.Plugins.CrossReferences;
using Doc2web.Plugins.Style;
using Doc2web.Plugins.TextProcessor;
using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web
{
    public static class QuickAndEasy
    {
        public static string ConvertCompleteDocument(WordprocessingDocument wpDoc)
        {
            using (var conversionEngine = new ConversionEngine(
                new StyleProcessorPlugin(wpDoc),
                new TextProcessorPlugin(),
                new CrossReferencesCleanupPlugin()))
            {
                return conversionEngine.Render(wpDoc.MainDocumentPart.Document.Body.Elements());
            }
        }
    }
}
