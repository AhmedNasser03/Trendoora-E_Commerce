using E_Commerce.Data.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTO
{
	public class ReviewDto
	{
		public double Rate { get; set; }
		public string? Comment { get; set; }
		public DateTime Date { get; set; }

		[SwaggerSchema(ReadOnly = true)]
		public string CustomerId { get; set; }
		public string ProductId { get; set; }
	}
}
