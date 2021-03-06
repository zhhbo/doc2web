﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using Doc2web.Plugins.Numbering;
using Doc2web.Plugins.Style;
using Doc2web.Plugins.TextFixes;
using Doc2web.Plugins.TextProcessor;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Benchmark
{
    [ClrJob]
    public class ConversionBenchmark
    {
        public ConversionEngine BuildConversionEngine()
        {
            return new ConversionEngine(
                new StylePlugin(_wpDoc),
                new TextProcessorPlugin(),
                new NumberingPlugin(_wpDoc),
                new CrossReferencesCleanupPlugin(),
                new HyphenInsertionPlugin(),
                new BreakInsertionPlugin(),
                new EscapeHtmlPlugin());
        }

        private WordprocessingDocument _wpDoc;
        private Paragraph[] _paragraphs;
        private Paragraph[] _shortestParagraph;
        private Paragraph[] _longestParagraph;
        private ConversionEngine _conversionEngine;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _wpDoc = WordprocessingDocument.Open(Utils.GetAssetPath("shareholders.docx"), false);
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
        public void GlobalCleanup()
        {
            _conversionEngine.Dispose();
            _wpDoc.Dispose();
        }

        //[Benchmark]
        public ConversionEngine InitializeEngine() => BuildConversionEngine();

        //[Benchmark]
        public string RenderShortest() => 
            _conversionEngine.ConvertToString(
                new StringConversionParameter { Elements = _shortestParagraph });

        //[Benchmark]
        public string RenderLongest() =>
            _conversionEngine.ConvertToString(
                new StringConversionParameter { Elements = _longestParagraph });

        [Benchmark]
        public string RenderComplete() =>
            _conversionEngine.ConvertToString(
                new StringConversionParameter { Elements = _paragraphs });

    }
}
