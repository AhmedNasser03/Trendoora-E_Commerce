using E_Commerce.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces
{
	public interface IOrderItemsService
	{
		Task<OrderItemsResultDto> GetOrderItemsByOrderId(string orderId);
		Task<bool> AddItemsToOrderAsync(OrderItemsDto orderItemsDto);
		Task<bool> DeleteItemFromOrderAsync(string orderId, string productId);
	}
}
