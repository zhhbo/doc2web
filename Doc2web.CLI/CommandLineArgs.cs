using Mono.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Doc2web.CLI
{
    class CommandLineArgs
    {
        private bool _shouldShowHelp;
        private int _skip;
        private int _take;
        private string _outputPath;
        private OptionSet _optionSet;
        private string[] _targets;
        private int _verbosity;
        private bool _interactive;
        private bool _blank;
        private string _find;
        private Regex _regex;

        public CommandLineArgs()
        {
            _skip = 0;
            _take = -1;
            _outputPath = "";
            _find = null;
            _regex = null;

            _optionSet = new OptionSet {
                { "s|skip=", "the number of paragraph to skip from the begining", n => _skip = int.Parse(n) },
                { "t|take=",   "the maximum of paragraph to render",               n => _take = int.Parse(n) },
                { "f|find=",  "a regex that fill filter out paragraphs using their contenxt",  n => _find = n },
                { "o|out=",   "the path of the output folder.",                    n => _outputPath = n },
                { "b|blank",  "toggle on/off if the html output will be written",  n => _blank = n != null },
                { "v|verbose",        "increase debug message verbosity",          v => { if (v != null) ++_verbosity; } },
                { "i|interactive", "will stop at the end of the execute and wait to the user to press enter", n => _interactive = n != null },
                { "h|help",   "show this message and exit",                        h => _shouldShowHelp = h != null },
            };

        }

        public bool IsInteractive => _interactive;

        public bool ShouldShowHelp => _shouldShowHelp;

        public int Skip => _skip;

        public int Take => _take;

        public Regex Regex => _regex;

        public bool Blank => _blank;

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

                if (_find != null)
                    _regex = new Regex(_find);
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
