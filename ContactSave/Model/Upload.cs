using System.ComponentModel.DataAnnotations;

namespace ContactSave.Model
{
    public class Upload
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Timestamp { get; set; }
        [Required]
        [MaxLength(100)]
        public string File { get; set; }
        [MaxLength(200)]
        public string Comment { get; set; }
    }
}
