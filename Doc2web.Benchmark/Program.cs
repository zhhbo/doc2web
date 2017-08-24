using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Running;
using DocumentFormat.OpenXml.Packaging;
using System;
using System.Security.Cryptography;

namespace Doc2web.Benchmark
{
    [ClrJob, CoreJob]
    public class SimpleConversion
    {
        WordprocessingDocument wpDoc = WordprocessingDocument.Open(@"C:\Users\osasseville\OneDrive - TermLynx\Desktop\Docs\transaction-formatted.docx", false);

        public SimpleConversion()
        {
        }

        [Benchmark]
        public string Convert() =>
            QuickAndEasy.ConvertCompleteDocument(wpDoc);

    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<SimpleConversion>();
        }
    }
}
