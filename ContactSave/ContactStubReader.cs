namespace ContactSave
{
    public class ContactStubReader : ObjectTableReader<ContactStub>
    {
        private readonly static string[] Headers = new[]
        {
            "Имя",
            "Фамилия",
            "Отчество",
            "Пол",
            "Дата рождения",
            "Город",
            "Категория",
            "Телефон"
        };

        public ContactStubReader(TextTableReader tableReader)
            : base(Headers, tableReader, CreateInstance)
        {
        }

        private static ContactStub CreateInstance(string[] values)
        {
            return new ContactStub
            {
                FirstName = values[0],
                LastName = values[1],
                Patronymic = values[2],
                Gender = values[3],
                BirthDate = values[4],
                City = values[5],
                Cathegory = values[6],
                Phone = values[7],
            };
        }

    }
}
