using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTO
{
	public class ProductDto
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public double Price { get; set; }
		public int StockAmount { get; set; }
		public double? DiscountPercentage { get; set; }
		public string CategoryId { get; set; }
		public string? BrandId { get; set; }
	}
}
