using ContactSave.Model;

using System.Globalization;
using System.Text.RegularExpressions;

namespace ContactSave
{
    public class ContactStub
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public string City { get; set; }
        public string Cathegory { get; set; }
        public string Phone { get; set; }

        public bool Validate(out Contact result)
        {
            var phone = CorrectPhone(Phone);
            if (phone.Length != 10)
            {
                result = null;
                return false;
            }
            if (!DateTime.TryParseExact(BirthDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var birthDate))
            {
                result = null;
                return false;
            }
            var city = CorrectCity(City);
            if (city.Length < 1)
                city = "не определен";
            City = city;
            result = new Contact 
            { 
                FirstName = FirstName,
                LastName = LastName,
                Patronymic = Patronymic,
                Gender = GetValidGender(Gender),
                BirthDate = birthDate,
                Phone = Phone
            };
            return true;
        }

        private static string? GetValidGender(string value)
        {
            return value switch
            {
                "м" or "ж" => value,
                _ => null,
            };
        }

        private static string CorrectPhone(string value)
        {
            return Regex.Replace(value, @"[^0-9]", string.Empty);
        }

        private static string CorrectCity(string value)
        {
            var leftTrimmed = Regex.Replace(value, @"^[^a-zA-Zа-яА-Я0-9]+", string.Empty);
            return Regex.Replace(leftTrimmed, @"[^a-zA-Zа-яА-Я0-9]+$", string.Empty);
        }
    }
}
