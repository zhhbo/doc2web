using DocumentFormat.OpenXml.Packaging;
using Mono.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Doc2web.CLI
{
    class Program
    {
        private static CommandLineArgs cliArgs;

        public static void Main(string[] args)
        {
            cliArgs = new CommandLineArgs();

            try
            {
                cliArgs.Parse(args);
            }
            catch
            {
                Debug("Could not parse arguments.");
                return;
            }

            if (cliArgs.ShouldShowHelp)
            {
                cliArgs.ShowHelp();
                return;
            }

            var targets = cliArgs.Targets.Where(IsValid).ToArray();
            if (targets.Length == 0)
            {
                Console.WriteLine("No valid document can be used as input.");
                return;
            }

            ConvertAllDocuments(targets);

            if (cliArgs.IsInteractive)
            {
                Console.WriteLine("Press enter to continue...");
                Console.ReadKey();
            }
        }

        private static void ConvertAllDocuments(string[] targets)
        {
            Debug("Starting the job");
            foreach (string target in targets)
            {
                ConvertDocument(target);
            }
            Debug("Job completed");
        }

        private static void ConvertDocument(string target)
        {
            string html;
            string fileName = Regex.Match(target, @"[^\\\/]+$").Value;
            string directory = target.Substring(0, fileName.Length - fileName.Length);
            Debug($"Starting {fileName}");
            using (var wpDoc = WordprocessingDocument.Open(target, false))
            {
                html = QuickAndEasy.ConvertCompleteDocument(wpDoc);
            };
            if (cliArgs.OutputPath != "") directory = cliArgs.OutputPath;
            File.WriteAllText($"{directory}{fileName}.html", html);
            Debug($"Completed conversion of {fileName}");
        }

        private static bool IsValid(string arg)
        {
            if (!File.Exists(arg))
            {
                Debug($"File {arg} does not exist");
                return false;
            }

            if (!Regex.IsMatch(arg, @"\.docx?$"))
            {
                Debug($"File {arg} does not end with `.doc` or `.docx`.");
                return false;
            }

            return true;
        }

        private static void Debug(string format, params object[] args)
        {
            if (cliArgs.Verbosity > 0)
            {
                Console.Write("# ");
                Console.WriteLine(format, args);
            }
        }
    }
}
