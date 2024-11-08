using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTO
{
	public class OrderItemsResultDto
	{
		public OrderResultDto Order { get; set; }
		public List<ProductResultDto> Products { get; set; }
	}
}
