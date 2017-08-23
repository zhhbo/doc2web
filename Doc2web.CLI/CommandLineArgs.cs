using Mono.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Doc2web.CLI
{
    class CommandLineArgs
    {
        private bool _shouldShowHelp;
        private int _start;
        private int _end;
        private string _outputPath;
        private OptionSet _optionSet;
        private string[] _targets;
        private int _verbosity;
        private bool _interactive;

        public CommandLineArgs()
        {
            _start = 0;
            _end = -1;
            _outputPath = "";

            _optionSet = new OptionSet {
                { "s|start=", "the number of paragraph to skip from the begining", n => _start = int.Parse(n) },
                { "e|end=",   "the number of paragraph to skip from the end",      n => _end = int.Parse(n) },
                { "o|out=",   "the path of the output folder.",                    n => _outputPath = n },
                { "v|verbose",        "increase debug message verbosity",                  v => { if (v != null) ++_verbosity; } },
                { "i|interactive", "will stop at the end of the execute and wait to the user to press enter", n => _interactive = n != null },
                { "h|help",   "show this message and exit",                        h => _shouldShowHelp = h != null },
            };

        }

        public bool IsInteractive => _interactive;

        public bool ShouldShowHelp => _shouldShowHelp;

        public int Start => _start;

        public int End => _end;

        public string OutputPath => _outputPath;

        public IReadOnlyList<string> Targets => _targets;

        public int Verbosity => _verbosity;

        public void Parse(string [] args)
        {
            _shouldShowHelp = args.Length == 0;
            if (_shouldShowHelp) return;

            try
            {
                _targets = 
                    _optionSet.Parse(args)
                    .Select(ParsePath)
                    .ToArray();
            }
            catch (OptionException e)
            {
                Console.Write("doc2web: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `doc2web.exe --help` or `netcore doc2web.cli.dll --help` for more information.");
                throw e;
            }
        }

        public void ShowHelp()
        {
            Console.WriteLine("Usage: doc2web.exe [OPTIONS]+ file1 file2 fileN");
            Console.WriteLine("Convert a list of docx/doc to html.");
            Console.WriteLine("If no message is specified, a generic greeting is used.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            _optionSet.WriteOptionDescriptions(Console.Out);
        }

        private static string ParsePath(string n)
        {
            if (Path.IsPathRooted(n)) return n;
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, n);
        }
    }
}
