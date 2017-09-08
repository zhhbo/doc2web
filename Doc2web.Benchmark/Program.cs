using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Running;
using Doc2web.Plugins.Style;
using Doc2web.Plugins.TextFixes;
using Doc2web.Plugins.TextProcessor;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Doc2web.Benchmark
{
    [ClrJob, CoreJob]
    public class SimpleConversion
    {
        public ConversionEngine BuildConversionEngine () => new ConversionEngine(
                new StyleProcessorPlugin(_wpDoc),
                new TextProcessorPlugin(),
                new CrossReferencesCleanupPlugin(),
                new HyphenInsertionPlugin(),
                new BreakInsertionPlugin(),
                new EscapeHtmlPlugin());

        private WordprocessingDocument _wpDoc;
        private Paragraph[] _paragraphs;
        private Paragraph[] _shortestParagraph;
        private Paragraph[] _longestParagraph;
        private ConversionEngine _conversionEngine;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _wpDoc = WordprocessingDocument.Open(@"C:\Users\osasseville\OneDrive - TermLynx\Desktop\Docs\tlshareholders.docx", false);
            _paragraphs = _wpDoc.MainDocumentPart.Document.Body.Elements<Paragraph>().ToArray();
            _shortestParagraph =
                _paragraphs.Where(x => x.InnerText.Length > 0)
                .OrderBy(x => x.InnerText.Length)
                .Take(1)
                .ToArray();
            _longestParagraph =
                _paragraphs.Where(x => x.InnerText.Length > 0)
                .OrderByDescending(x => x.InnerText.Length)
                .Take(1)
                .ToArray();

            Console.WriteLine($"Total: {_paragraphs.Length}, shortest char: ${_shortestParagraph[0].InnerText.Length}, longest char: {_longestParagraph[0].InnerText.Length}");

            _conversionEngine = BuildConversionEngine();
        }

        [GlobalCleanup]
        public void GlobalCleanup ()
        {
            _conversionEngine.Dispose();
            _wpDoc.Dispose();
        }

        [Benchmark]
        public ConversionEngine InitializeEngine() => BuildConversionEngine();

        [Benchmark]
        public string RenderShortest() => _conversionEngine.Render(_shortestParagraph);

        [Benchmark]
        public string RenderLongest() => _conversionEngine.Render(_longestParagraph);

        [Benchmark]
        public string RenderComplete() => _conversionEngine.Render(_paragraphs);

    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<SimpleConversion>();
            Console.ReadLine();
        }
    }
}
