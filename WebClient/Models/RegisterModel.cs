using System.ComponentModel.DataAnnotations;

namespace EventsAPI.Models
{
    public class RegisterModel
    {

        public string Name { get; set; }

        public string Surname { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}",ApplyFormatInEditMode = true)]
        public DateOnly BirthDate { get; set; }

        public string Email { get; set; }
    }
}
