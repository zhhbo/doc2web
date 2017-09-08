using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Doc2web.Benchmark
{
    public static class Utils
    {
        public static string GetAssetPath (string assetFileName) =>
            Path.Combine(
              Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
              "Assets",
              assetFileName
            );
    }
}
