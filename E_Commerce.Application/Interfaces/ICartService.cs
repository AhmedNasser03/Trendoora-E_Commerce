using E_Commerce.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces
{
	public interface ICartService
	{
		Task<CartResultDto> GetCartById(string id);
		Task<IEnumerable<CartResultDto>> GetAllCartsAsync();
		Task<bool> UpdateCartAsync(string cartId, CartDto cartDto);
		Task<bool> AddCartAsync(CartDto cartDto);
		Task<bool> DeleteCartAsync(string id);
	}
}
