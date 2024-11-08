namespace E_Commerce.Application.DTO
{
	public class PaymentResultDto
	{
		public DateTime Date { get; set; }
		public string Method { get; set; }
		public decimal Amount { get; set; }
		public UserResultDto Customer { get; set; }
	}
}
