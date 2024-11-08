using E_Commerce.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces
{
	public interface ICartItemsService
	{
		Task<CartItemsResultDto> GetCartItemsByCartId(string cartId);
		Task<bool> AddItemsToCartAsync(CartItemsDto cartItemsDto);
		Task<bool> DeleteItemFromCartAsync(string cartId, string productId);
	}
}
