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
            var cliArgs = new CommandLineArgs();

            try
            {
                cliArgs.Parse(args);
            }
            catch
            {
                Console.Error.WriteLine("Could not parse arguments.");
                return;
            }

            if (cliArgs.ShouldShowHelp)
            {
                cliArgs.ShowHelp();
                return;
            }

            new Program(cliArgs).Execute();

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
    }
}
