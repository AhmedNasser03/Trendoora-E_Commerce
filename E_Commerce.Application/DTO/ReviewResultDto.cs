using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTO
{
	public class ReviewResultDto
	{
		public double Rate { get; set; }
		public string? Comment { get; set; }
		public DateTime Date { get; set; }
		public UserResultDto Customer { get; set; }
		public ProductResultDto Product { get; set; }
	}
}
