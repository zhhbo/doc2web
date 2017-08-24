namespace Doc2web.Core
{
    public interface IContextNestingHandler
    {
        void QueueElementProcessing(INestableElementContext context);
    }
}