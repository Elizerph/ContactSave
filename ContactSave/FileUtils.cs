namespace ContactSave
{
    public static class FileUtils
    {
        public static async IAsyncEnumerable<string> ReadLinesAsync(string file)
        {
            using var reader = new StreamReader(file);
            string? line = null;
            while ((line = await reader.ReadLineAsync()) != null)
                yield return line;
        }
    }
}
