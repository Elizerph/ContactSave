using ContactSave.Model;

using System.Data;

namespace ContactSave
{
    public class DataGridDataConsumer : IDataConsumer<Tuple<ContactStub, Contact>>
    {
        private readonly DataGridView _grid;
        private DataTable? _table;

        public DataGridDataConsumer(DataGridView grid)
        { 
            _grid = grid ?? throw new ArgumentNullException(nameof(grid));
        }

        public async Task Init()
        {
            var prevSourceDisposable = _grid.DataSource as IDisposable;
            var prevSourceAsyncDisposable = _grid.DataSource as IAsyncDisposable;
            _grid.DataSource = null;
            prevSourceDisposable?.Dispose();
            if (prevSourceAsyncDisposable != null)
                await prevSourceAsyncDisposable.DisposeAsync();

            var columns = new DataColumn[]
            {
                new DataColumn("FirstName", typeof(string)),
                new DataColumn("LastName", typeof(string)),
                new DataColumn("Patronymic", typeof(string)),
                new DataColumn("Gender", typeof(string)),
                new DataColumn("BirthDate", typeof(DateTime)),
                new DataColumn("City", typeof(string)),
                new DataColumn("Cathegory", typeof(string)),
                new DataColumn("Phone", typeof(string)),
            };
            _table = new DataTable();
            _table.Columns.AddRange(columns);
        }

        public Task Consume(Tuple<ContactStub, Contact> data)
        {
            if (_table != null)
            {
                var row = _table.NewRow();
                row["FirstName"] = data.Item1.FirstName;
                row["LastName"] = data.Item1.LastName;
                row["Patronymic"] = data.Item1.Patronymic;
                row["Gender"] = data.Item1.Gender;
                row["BirthDate"] = data.Item1.BirthDate;
                row["City"] = data.Item1.City;
                row["Cathegory"] = data.Item1.Cathegory;
                row["Phone"] = data.Item1.Phone;
                _table.Rows.Add(row);
            }
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _grid.DataSource = _table;
            _table = null;
        }
    }
}
