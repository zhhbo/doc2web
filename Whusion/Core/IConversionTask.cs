namespace Whusion.Core
{
    public interface IConversionTask
    {
        void AssembleDocument();
        void ConvertElements();
        void PostProcess();
        void PreProcess();

        string Result { get; }
    }
}