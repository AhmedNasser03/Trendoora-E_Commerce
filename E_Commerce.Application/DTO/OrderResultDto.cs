using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTO
{
	public class OrderResultDto
	{
		public DateTime Date { get; set; }
		public string Status { get; set; }
		public string ShippingAddress { get; set; }
		public string ShippingMethod { get; set; }
		public DateTime ShippingDate { get; set; }
		public DateTime DeliveringDate { get; set; }
		public UserResultDto Customer { get; set; }
	}
}
