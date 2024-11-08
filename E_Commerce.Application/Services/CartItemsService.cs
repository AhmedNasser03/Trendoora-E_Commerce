using AutoMapper;
using E_Commerce.Application.DTO;
using E_Commerce.Application.Helpers;
using E_Commerce.Application.Interfaces;
using E_Commerce.Data.Models;
using E_Commerce.Infrastructure.IGenericRepository_IUOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
	public class CartItemsService : ICartItemsService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserHelpers _userHelpers;
		private readonly IMapper _mapper;
		public CartItemsService(IUnitOfWork unitOfWork, IMapper mapper, IUserHelpers userHelpers)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_userHelpers = userHelpers;
		}

		public async Task<CartItemsResultDto> GetCartItemsByCartId(string cartId)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var desireListItems = await _unitOfWork.CartItems.FindAsync(c => c.CartId == cartId, ["Cart", "Product"]);

			if (desireListItems == null || !desireListItems.Any())
				throw new Exception("Cart not found");
			if (currentUser == null) throw new Exception("not allowed to get this Cart");
			var result = new CartItemsResultDto
			{
				Cart = _mapper.Map<CartResultDto>(desireListItems.First().Cart),
				Products = desireListItems.Select(d => _mapper.Map<ProductResultDto>(d.Product)).ToList()
			};
			return result;
		}

		public async Task<bool> AddItemsToCartAsync(CartItemsDto cartItemsDto)
		{
			var cartItem = _mapper.Map<CartItems>(cartItemsDto);
			await _unitOfWork.CartItems.Add(cartItem);
			if (await _unitOfWork.SaveAsync() > 0)
				return true;
			return false;
		}

		public async Task<bool> DeleteItemFromCartAsync(string cartId, string productId)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var cartItem = await _unitOfWork.CartItems.FindFirstAsync(p => p.CartId == cartId && p.ProductId == productId);
			if (cartItem == null) throw new Exception("CartItem not found");
			if (currentUser == null)
				throw new Exception("not allowed to delete");
			await _unitOfWork.CartItems.Remove(cartItem);
			if (await _unitOfWork.SaveAsync() > 0)
			{
				return true;
			}
			return false;
		}
	}
}
