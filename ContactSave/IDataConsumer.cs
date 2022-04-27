namespace ContactSave
{
    public interface IDataConsumer<T> : IDisposable
    {
        Task Init();
        Task Consume(T data);
    }
}
