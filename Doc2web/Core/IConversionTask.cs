namespace Doc2web.Core
{
    public interface IConversionTask
    {
        void AssembleDocument();

        void Initialize();

        void PreProcess();

        void ConvertElements();

        void PostProcess();

        string Result { get; }
    }
}