namespace Doc2web.Core
{
    public interface IProcessorFactory
    {
        Processor BuildMultiple(params object[] inputObject);
        Processor BuildSingle(object plugin);
    }
}