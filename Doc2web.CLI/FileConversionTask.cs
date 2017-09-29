using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Doc2web.CLI
{
    public class FileConversionTask
    {
        private string html;
        private Stopwatch _sw;
        private long _ms;
        private string _directory;

        public bool Blank { get; set; }

        public string InputPath { get; set; }

        public string OutputPath { get; set; }

        public bool Verbose { get; set; }

        private string FileName => Regex.Match(InputPath, @"[^\\\/]+$").Value;

        public void Execute()
        {
            _ms = 0;
            _directory = InputPath.Substring(0, InputPath.Length - FileName.Length);

            Debug($"Starting {FileName}");

            TryConvert();
            if (!Blank) WriteOutput();

            Debug($"Completed conversion of {FileName}");

            Console.WriteLine($"Converted {FileName} in {_ms} ms");
        }

        private void WriteOutput()
        {
            if (OutputPath != "")
            {
                if (Path.IsPathRooted(OutputPath)) _directory = OutputPath;
                else _directory = Path.Combine(_directory, OutputPath);
                if (!Directory.Exists(_directory)) Directory.CreateDirectory(_directory);
            }
            File.WriteAllText($"{_directory}{FileName}.html", html);
        }

        private void TryConvert()
        {
            try
            {
                using (var wpDoc = WordprocessingDocument.Open(InputPath, false))
                {
                    ConvertDocument(wpDoc);
                };
            } catch (Exception ex)
            {
                Console.Error.WriteLine($"Could not open {FileName}");
            }
        }

        private void ConvertDocument(WordprocessingDocument wpDoc)
        {
            _sw = Stopwatch.StartNew();
            try
            {
                html = QuickAndEasy.ConvertCompleteDocument(wpDoc);
            }
            catch (Exception ex)
            {
                html = "Conversion error";
                Console.Error.WriteLine($"Failed to convert ${FileName}");
            } finally
            {
                _sw.Stop();
                _ms = _sw.ElapsedMilliseconds;
            }
        }

        private void Debug(string format, params object[] args)
        {
            if (Verbose)
            {
                Console.Write("# ");
                Console.WriteLine(format, args);
            }
        }
    }
}
