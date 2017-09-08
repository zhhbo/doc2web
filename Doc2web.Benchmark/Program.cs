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
    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<ConversionBenchmark>();
            Console.ReadLine();
        }
    }
}
