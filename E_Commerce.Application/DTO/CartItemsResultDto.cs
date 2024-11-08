using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTO
{
	public class CartItemsResultDto
	{
		public CartResultDto Cart { get; set; }
		public List<ProductResultDto> Products { get; set; }
	}
}
