using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Data.Models
{
	public class Order
    {
        [Key, MaxLength(200)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingMethod { get; set; }
        public DateTime ShippingDate { get; set; }
        public DateTime DeliveringDate { get; set; }
        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }

		public virtual ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();

	}
}
