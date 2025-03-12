using System.ComponentModel.DataAnnotations;

namespace EventsAPI.Models
{
    public class RegisterModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}",ApplyFormatInEditMode = true)]
        public DateOnly BirthDate { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
