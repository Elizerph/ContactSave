using System.ComponentModel.DataAnnotations;

namespace ContactSave.Model
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }
        [MaxLength(30)]
        public string Patronymic { get; set; }
        [MaxLength(30)]
        public string? Gender { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        public City? City { get; set; }
        public Cathegory? Cathegory { get; set; }
        [Phone]
        public string Phone { get; set; }
    }
}
