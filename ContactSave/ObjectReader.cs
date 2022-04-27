namespace ContactSave
{
    public class ObjectTableReader<T>
    {
        private readonly string[] _headers;
        private readonly TextTableReader _tableReader;
        private readonly Func<string[], T> _createInstance;

        public ObjectTableReader(string[] headers, TextTableReader tableReader, Func<string[], T> createInstance)
        {
            _headers = headers ?? throw new ArgumentNullException(nameof(headers));
            _createInstance = createInstance ?? throw new ArgumentNullException(nameof(createInstance));
            _tableReader = tableReader ?? throw new ArgumentNullException(nameof(tableReader));
        }

        public IAsyncEnumerable<T> Read(IAsyncEnumerable<string> lines)
        { 
            return _tableReader.Read(_headers, lines).Select(_createInstance);
        }
    }
}
