using E_Commerce.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
		public string LastName { get; set; }
		public string? UserImage { get; set; }
        public string Type { get; set; }
        public string? OTP { get; set; }
        public DateTime? OTPExpiry { get; set; }
		public int ActiveDevices { get; set; } = 0;

		[ForeignKey("Cart")]
        public string? CartID { get; set; }
        public Cart? Cart { get; set; }
        public virtual ICollection<Reviews>? Reviews { get; set; }
		public List<RefreshToken>? RefreshTokens { get; set; }
		public virtual ICollection<Payment>? Payments { get; set; }

    }
}
