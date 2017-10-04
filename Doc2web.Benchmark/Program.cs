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
using Doc2web.Plugins.Style.Css;
using System.Text;

namespace Doc2web.Benchmark
{
    public class Program
    {

        public class CssComparer : IEqualityComparer<CssClass>
        {
            public bool Equals(CssClass x, CssClass y)
            {
                return GetCss(x) == GetCss(y);
            }

            public int GetHashCode(CssClass obj)
            {
                return GetCss(obj).GetHashCode();
            }

            private string GetCss(CssClass obj)
            {
                var cssData = new CssData();
                obj.Name = "mockcls";
                var sb = new StringBuilder();
                obj.InsertCss(cssData);
                cssData.RenderInto(sb);
                return sb.ToString();
            }
        }

        public static void Main(string[] args)
        {
            var switcher = new BenchmarkSwitcher(new[] {
                typeof(ConversionBenchmark),
                typeof(RenderingBenchmark),
                typeof(StyleBenchmark),
            });
            switcher.Run(args);

            Console.ReadKey();
        }
    }
}
