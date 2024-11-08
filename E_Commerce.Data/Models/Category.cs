using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Data.Models
{
    public class Category
    {
        [Key, MaxLength(200)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public virtual ICollection<Product>? Products { get; set; }

    }
}
