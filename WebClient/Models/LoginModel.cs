using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebClient.Models
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }
        [AllowNull]
        public string? Code { get; set; }
    }
}
