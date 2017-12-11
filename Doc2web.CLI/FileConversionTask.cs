using DocumentFormat.OpenXml;
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

        private int _skip = 0;
        private int _take = -1;
        private Regex _regex = null;
        private string _outputPath;
        private ConversionEngine _conversionEngine;
        private FileStream _outputStream;
        private WordprocessingDocument _wpDoc;

        public bool Blank { get; set; }

        public string InputPath { get; set; }

        public string OutputPath { get; set; }

        public bool Verbose { get; set; }

        private string FileName => Regex.Match(InputPath, @"[^\\\/]+$").Value;

        public int Skip { get => _skip; set => _skip = value; }
        public int Take { get => _take; set => _take = value; }
        public Regex Regex { get => _regex; set => _regex = value; }

        public void Execute()
        {
            _ms = 0;
            _directory = InputPath.Substring(0, InputPath.Length - FileName.Length);
            if (Blank) return;

            Debug($"Starting {FileName}");

            SelectOutput();
            TryConvert();

            Debug($"Completed conversion of {FileName}");

            Console.WriteLine($"Converted {FileName} in {_ms} ms");
        }

        private void SelectOutput()
        {
            if (OutputPath != "")
            {
                if (Path.IsPathRooted(OutputPath)) _directory = OutputPath;
                else _directory = Path.Combine(_directory, OutputPath);
                if (!Directory.Exists(_directory)) Directory.CreateDirectory(_directory);
            }
            _outputPath = $"{_directory}{FileName}.html";
        }

        private void TryConvert()
        {
            try
            {
                using (_wpDoc = WordprocessingDocument.Open(InputPath, false))
                using (_conversionEngine = QuickAndEasy.BuildDefaultEngine(_wpDoc))
                using (_outputStream = File.OpenWrite(_outputPath))
                {
                    ConvertDocument();
                };
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Could not open {FileName}");
            }
        }

        private void ConvertDocument()
        {
            _sw = Stopwatch.StartNew();
            try
            {
                var elems = FindElements(_wpDoc);
                _conversionEngine.Convert(new ConversionParameter
                {
                    Elements = elems,
                    Stream = _outputStream
                });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert ${FileName}");
            } finally
            {
                _sw.Stop();
                _ms = _sw.ElapsedMilliseconds;
            }
        }

        private OpenXmlElement[] FindElements(WordprocessingDocument wpDoc)
        {
            var elements = wpDoc.MainDocumentPart.Document.Body.Elements();

            if (Regex != null)
                elements = elements.Where(x => Regex.IsMatch(x.InnerText));

            elements = elements.Skip(Skip);
            if (Take != -1) elements = elements.Take(Take);

            return elements.ToArray();
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
