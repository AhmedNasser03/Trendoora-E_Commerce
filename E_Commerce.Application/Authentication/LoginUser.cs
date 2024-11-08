using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Authentication
{
    public class LoginUser
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
