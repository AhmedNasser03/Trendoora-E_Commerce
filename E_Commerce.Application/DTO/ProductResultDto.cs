using E_Commerce.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTO
{
	public class ProductResultDto
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public double Price { get; set; }
		public int StockAmount { get; set; }
		public double? DiscountPercentage { get; set; }

	}
}
