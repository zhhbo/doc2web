using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using Mono.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Doc2web.CLI
{
    class Program
    {
        private static CommandLineArgs cliArgs;
        private CommandLineArgs _args;
        private FileListFactory _fileList;

        public static void Main(string[] args)
        {
            //var cliArgs = new CommandLineArgs();

            //try
            //{
            //    cliArgs.Parse(args);
            //}
            //catch
            //{
            //    Console.Error.WriteLine("Could not parse arguments.");
            //    return;
            //}

            //if (cliArgs.ShouldShowHelp)
            //{
            //    cliArgs.ShowHelp();
            //    return;
            //}

            //new Program(cliArgs).Execute();

            var t = new Program(null);
            t.Test();

            Console.WriteLine("Press enter to continue...");
            Console.ReadKey();
        }

        private Program(CommandLineArgs args)
        {
            _args = args;
        }

        private void Execute()
        {
            var fileListFactory = new FileListFactory(_args.Targets.ToArray());
            var fileList = fileListFactory.Build();

            var sw = Stopwatch.StartNew();

            foreach (var file in fileList)
            {
                var task = new FileConversionTask
                {
                    InputPath = file,
                    OutputPath = _args.OutputPath,
                    Verbose = _args.Verbosity > 1,
                    Blank = _args.Blank,
                    Skip = _args.Skip,
                    Take = _args.Take,
                    Regex = _args.Regex
                };

                task.Execute();
            }

            Console.WriteLine(@"-----------------------------------------------------");
            Console.WriteLine($"Converted {fileList.Count} document(s) in {sw.Elapsed.ToString()}.");
        }

        string sentence = "“Articles” means the articles of incorporation of the Corporation, as amended, replaced, restated or supplemented from time to time;";


        string[] spans = new string[]
        {
            "“",
            "Articles",
            "” means the articles of incorporation of the ",
            "Corporation",
            ", as amended, replaced, restated or supplemented from time to time;"
        };
        string[] highlights = new string[]
        {
            "Articles"
        };
        string[] anchors = new string[] {
            "Articles",
            "Corporation"
        };

        [ElementProcessing]
        public void ProcessP(IElementContext ctx, Paragraph p)
        {
            ctx.AddNode(new HtmlNode { Start = 0, End = p.InnerText.Length, Tag = "p" });
            foreach(string span in spans)
            {
                ctx.AddNode(new HtmlNode
                {
                    Start = sentence.IndexOf(span),
                    End = sentence.IndexOf(span) + span.Length,
                    Z = 0,
                    Tag = "span"
                });
            }

            foreach(string highlight in highlights)
            {
                var node = new HtmlNode
                {
                    Start = sentence.IndexOf(highlight),
                    End = sentence.IndexOf(highlight) + highlight.Length,
                    Z = 5,
                    Tag = "span"
                };
                node.SetStyle("background", "yellow");
                ctx.AddNode(node);
            }

            foreach(string anchor in anchors)
            {
                var node = new HtmlNode
                {
                    Start = sentence.IndexOf(anchor),
                    End = sentence.IndexOf(anchor) + anchor.Length,
                    Z = 10,
                    Tag = "a"
                };
                ctx.AddNode(node);
            }
        }

        public void Test()
        {
            var conversionEngine = new ConversionEngine(this);
            var html = conversionEngine.Convert(new Paragraph[1] { new Paragraph(new Run(new Text(sentence))) });
            Console.WriteLine(html);
        }

    }
}
