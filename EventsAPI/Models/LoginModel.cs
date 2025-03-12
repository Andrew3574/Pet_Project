using System.ComponentModel.DataAnnotations;

namespace EventsAPI.Models
{
    public class LoginModel
    {
        
        [Required]
        public string Email { get; set; }
        public string? ConfirmationPassword { get; set; }
    }
}
