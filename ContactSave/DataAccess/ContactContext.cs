using ContactSave.Model;

using Microsoft.EntityFrameworkCore;

namespace ContactSave.DataAccess
{
    public class ContactContext : DbContext
    {
        public ContactContext()
        { }

        public DbSet<Upload> Uploads { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Cathegory> Cathegories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ryuji\source\repos\ContactSave\ContactSave\ContactDb.mdf;Integrated Security=True";
#else
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);
            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ContactDb.mdf;Integrated Security=True";
#endif
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
