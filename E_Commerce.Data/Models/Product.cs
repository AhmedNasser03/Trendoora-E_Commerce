using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Data.Models
{
    public class Product
    {
        [Key, MaxLength(200)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image {  get; set; }

        public double Price { get; set; }
        public int StockAmount { get; set; }
        public double? DiscountPercentage { get; set; } = 0.00;
        [ForeignKey("Category")]
        public string CategoryId { get; set; }
        public Category Category { get; set; }
        [ForeignKey("Brand")]
        public string? BrandId {  get; set; }
        public Brand? Brand { get; set; }
    }
}
