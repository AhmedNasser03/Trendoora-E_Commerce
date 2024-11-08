using Swashbuckle.AspNetCore.Annotations;

namespace E_Commerce.Application.DTO
{
	public class PaymentDto
	{
		public DateTime Date { get; set; }
		public string Method { get; set; }
		public decimal Amount { get; set; }
		[SwaggerSchema(ReadOnly = true)]
		public string CustomerId { get; set; }
	}
}
