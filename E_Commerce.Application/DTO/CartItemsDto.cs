using E_Commerce.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTO
{
	public class CartItemsDto
	{
		public int Quantity { get; set; }
		public string ProductId { get; set; }
		public string CartId { get; set; }
	}
}
