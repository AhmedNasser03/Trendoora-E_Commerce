using E_Commerce.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces
{
	public interface IPaymentService
	{
		Task<PaymentResultDto> GetPaymentById(string id);
		Task<IEnumerable<PaymentResultDto>> GetAllPayemntsAsync();
		Task<bool> AddPaymentAsync(PaymentDto cartDto);
		Task<bool> DeletePaymentAsync(string id);
		Task<IEnumerable<string>> GetAllPaymentMethodsAsync();

	}
}
