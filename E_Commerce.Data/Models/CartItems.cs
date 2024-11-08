using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Data.Models
{
    public class CartItems
    {
        public int Quantity { get; set; }
        [ForeignKey("Product")]
        public string ProductId { get; set; }
        public Product Product { get; set; }
        [ForeignKey("Cart")]
        public string CartId { get; set; }
        public Cart Cart { get; set; }
    }

}
