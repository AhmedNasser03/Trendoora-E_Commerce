using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Data.Models
{
    public class Reviews
    {
        [Key, MaxLength(200)]
        public string Id { get; set; } = Guid.NewGuid().ToString(); 
        public double Rate { get; set; }
        public string? Comment { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }
        [ForeignKey("Product")]
        public string ProductId { get; set; }
        public Product Product { get; set; }

    }
}
