using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Data.Models
{
    public class OrderItems
    {
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        [ForeignKey("Product")]
        public string ProductId { get; set; }
        public Product Product { get; set; }

        [ForeignKey("Order")]
        public string OrderId { get; set; }
        public Order Order { get; set; }
    }
}
