using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Data.Models
{
    public class Payment
    {
        [Key, MaxLength(200)]
        public string Id { get; set; } = Guid.NewGuid().ToString(); 
        public DateTime Date { get; set; }
        public string Method { get; set; }
        public decimal Amount { get; set; } 

        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }


    }
}
