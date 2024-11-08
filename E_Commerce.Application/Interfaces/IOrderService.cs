using E_Commerce.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces
{
	public interface IOrderService
	{
		Task<OrderResultDto> GetOrderById(string id);
		Task<IEnumerable<OrderResultDto>> GetAllOrdersAsync();
		Task<bool> UpdateOrderAsync(string orderId, OrderDto orderDto);
		Task<bool> AddOrderAsync(OrderDto orderDto);
		Task<IEnumerable<string>> GetAllShippingMethodsAsync();
		Task<bool> DeleteOrderAsync(string id);
	}
}
