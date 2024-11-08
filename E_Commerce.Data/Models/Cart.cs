using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Data.Models
{
    public class Cart
    {
        [Key, MaxLength(200)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public decimal TotalPrice { get; set; }
        public string? Coupon {  get; set; }

        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }
		public virtual ICollection<CartItems> CartItems { get; set; } = new List<CartItems>();

	}

}
