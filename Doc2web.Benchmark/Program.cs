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

        public class CssComparer : IEqualityComparer<CssClass2>
        {
            public bool Equals(CssClass2 x, CssClass2 y)
            {
                return GetCss(x) == GetCss(y);
            }

            public int GetHashCode(CssClass2 obj)
            {
                return GetCss(obj).GetHashCode();
            }

            private string GetCss(CssClass2 obj)
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
                typeof(Style2Benchmark),
            });
            switcher.Run(args);

            //var b = new Style2Benchmark();
            //b.Setup();
            //b.ResetContainer();
            //b.RenderAllRunStyles();

            //var rs = b.Registrations;
            //var comparer = new CssComparer();
            //var set = new HashSet<CssClass2>(comparer);

            //Console.WriteLine($"Registration count: {rs.Count()}");

            //foreach (var c in rs) set.Add(c);

            //Console.WriteLine($"Unique props count: {set.Count()}");


            //foreach(var e1 in rs)
            //{
            //    foreach (var e2 in rs)
            //    {
            //        if (comparer.Equals(e1, e2) &&
            //            !e1.Props.SetEquals(e2.Props) && 
            //            e1 != e2)
            //        {
            //            e1.Props.SetEquals(e2.Props);
            //        }
            //    }
            //}

            //var cssData = new CssData();
            //var sb = new StringBuilder();

            //foreach(var cls in set.OrderBy(x => x.Props.Count))
            //{
            //    cls.Name = Guid.NewGuid().ToString();
            //    cls.InsertCss(cssData);
            //}

            //cssData.RenderInto(sb);

            //Console.WriteLine(sb);

            //foreach (var g in rs.GroupBy(x => x.Props.Count))
            //{
            //    Console.WriteLine($"{g.Key} : {g.Count()}");
            //    foreach(var sg in g.GroupBy(x => CombineHashCodes(x.Props.Select(y => y.GetHashCode()))))
            //    {
            //        Console.WriteLine($"{g.Key},{sg.Key} : {sg.Count()}\t{");

            //    }
            //}

            Console.ReadKey();
        }
    }
}
