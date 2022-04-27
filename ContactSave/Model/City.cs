using System.ComponentModel.DataAnnotations;

namespace ContactSave.Model
{
    public class City
    {
        [Key]
        public string Name { get; set; }
    }
}
