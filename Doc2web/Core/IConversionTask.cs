using System.IO;

namespace Doc2web.Core
{
    public interface IConversionTask
    {
        void AssembleDocument();

        void PreProcess();

        void ProcessElements();

        void PostProcess();

        StreamWriter Out { get; set; }
    }
}