using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTO
{
	public class CartDto
	{
		public decimal TotalPrice { get; set; }
		public string? Coupon { get; set; }
		public string CustomerId { get; set; }
	}
}
