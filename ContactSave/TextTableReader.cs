namespace ContactSave
{
    public class TextTableReader
    {
        private readonly Func<string, string[]> _getValues;

        public TextTableReader(Func<string, string[]> getValues)
        {
            _getValues = getValues ?? throw new ArgumentNullException(nameof(getValues));
        }

        public async IAsyncEnumerable<string[]> Read(string[] headers, IAsyncEnumerable<string> lines)
        {
            await using var enumerator = lines.GetAsyncEnumerator();
            if (await enumerator.MoveNextAsync())
            {

                var sourceHeaders = _getValues(enumerator.Current);
                var map = Enumerable.Repeat(-1, headers.Length).ToArray();
                for (var i = 0; i < headers.Length; i++)
                {
                    var header = headers[i];
                    for (var j = 0; j < sourceHeaders.Length; j++)
                        if (string.Equals(header, sourceHeaders[j], StringComparison.InvariantCultureIgnoreCase))
                        {
                            map[i] = j;
                            break;
                        }
                }
                if (map.Any(e => e == -1))
                    yield break;
                while (await enumerator.MoveNextAsync())
                {
                    var currentValues = _getValues(enumerator.Current);
                    var result = new string[headers.Length];
                    for (int i = 0; i < map.Length; i++)
                        result[i] = currentValues[map[i]];
                    yield return result;
                }
            }
        }

        public static TextTableReader GetSemicolon()
        {
            return new TextTableReader(line => line.Split(';'));
        }
    }
}
