namespace ContactSave
{
    public class CompositeDataConsumer<T> : IDataConsumer<T>
    {
        private readonly List<IDataConsumer<T>> _consumers = new();

        public Task Init()
        {
            return Task.WhenAll(_consumers.Select(e => e.Init()));
        }

        public Task Consume(T data)
        {
            return Task.WhenAll(_consumers.Select(e => e.Consume(data)));
        }

        public void AddConsumer(IDataConsumer<T> consumer)
        { 
            _consumers.Add(consumer);
        }

        public void Dispose()
        {
            foreach (var consumer in _consumers)
                consumer.Dispose();
        }
    }
}
