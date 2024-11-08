using Swashbuckle.AspNetCore.Annotations;

namespace E_Commerce.Application.DTO
{
	public class DesireListDto
	{
		public string Name { get; set; }
		[SwaggerSchema(ReadOnly = true)]
		public string CustomerId { get; set; }
	}
}
