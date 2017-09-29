using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Doc2web.CLI
{
    public class FileListFactory
    {
        private string[] targets;

        public FileListFactory(string[] targets)
        {
            this.targets = targets;
        }

        public List<string> Build()
        {
            var files = new List<string>();
            foreach(var target in targets)
            {
                if (IsDocFile(target)) files.Add(target);
                else files.AddRange(DocFileInDirectory(target));
            }
            return files;
        }

        private IEnumerable<string> DocFileInDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Console.Error.WriteLine($"The directory ${path} does not exist.");
                return Enumerable.Empty<string>();
            }
            return Directory.EnumerateFiles(path).Where(IsDocFile);
        }

        private static bool IsDocFile(string target)
        {
            return Regex.IsMatch(target, @"\.docx?$");
        }
    }
}
