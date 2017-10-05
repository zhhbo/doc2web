namespace Doc2web.Core
{
    public interface IConversionTask
    {
        void AssembleDocument();

        void Initialize();

        void PreProcess();

        void ProcessElements();

        void PostProcess();

        string Result { get; }
    }
}