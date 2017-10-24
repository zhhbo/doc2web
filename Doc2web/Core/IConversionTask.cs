namespace Doc2web.Core
{
    public interface IConversionTask
    {
        void AssembleDocument();

        void PreProcess();

        void ProcessElements();

        void PostProcess();

        string Result { get; }
    }
}