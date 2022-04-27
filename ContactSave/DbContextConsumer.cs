using ContactSave.DataAccess;
using ContactSave.Model;

namespace ContactSave
{
    public class DbContextConsumer : IDataConsumer<Tuple<ContactStub, Contact>>
    {
        private readonly ContactContext _context;

        public DbContextConsumer(ContactContext context)
        { 
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task Init()
        {
            return Task.CompletedTask;
        }

        public async Task Consume(Tuple<ContactStub, Contact> data)
        {
            var city = await _context.Cities.FindAsync(data.Item1.City);
            if (city == null)
                city = new City { Name = data.Item1.City };

            var cathegory = await _context.Cathegories.FindAsync(data.Item1.Cathegory);
            if (cathegory == null)
                cathegory = new Cathegory { Name = data.Item1.Cathegory };

            data.Item2.City = city;
            data.Item2.Cathegory = cathegory;
            await _context.Contacts.AddAsync(data.Item2);
        }

        public void Dispose()
        {
        }
    }
}
