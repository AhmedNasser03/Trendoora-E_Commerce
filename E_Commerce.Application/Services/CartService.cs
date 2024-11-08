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
	public class CartService : ICartService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserHelpers _userHelpers;
		private readonly IMapper _mapper;
		public CartService(IUnitOfWork unitOfWork, IMapper mapper, IUserHelpers userHelpers)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_userHelpers = userHelpers;
		}

		public async Task<CartResultDto> GetCartById(string id)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var cart = await _unitOfWork.Cart.FindFirstAsync(c => c.Id == id , "Customer");
			if (cart == null) throw new Exception("Cart not found");
			if (currentUser == null) throw new Exception("not allowed to get this Cart");
			var result = _mapper.Map<CartResultDto>(cart);
			return result;
		}
		public async Task<bool> AddCartAsync(CartDto cartDto)
		{
			var cart = _mapper.Map<Cart>(cartDto);
			await _unitOfWork.Cart.Add(cart);
			if (await _unitOfWork.SaveAsync() > 0)
				return true;
			return false;
		}
		public async Task<IEnumerable<CartResultDto>> GetAllCartsAsync()
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var carts = await _unitOfWork.Cart.FindAsync(f => true, "Customer");
			if (carts == null) throw new Exception("Cart not found");
			if (currentUser == null) throw new Exception("not allowed to get this Cart");
			var result = carts.Select(_mapper.Map<CartResultDto>).ToList();
			return result;
		}
		public async Task<bool> UpdateCartAsync(string cartId, CartDto cartDto)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var cart = await _unitOfWork.Cart.FindFirstAsync(c => c.Id == cartId);
			if (cart == null) throw new Exception("Cart not found");
			if (currentUser == null) throw new Exception("not allowed to update");

			_mapper.Map(cartDto, cart);
			await _unitOfWork.Cart.UpdateAsync(cart);
			if (await _unitOfWork.SaveAsync() > 0)
				return true;
			return false;
		}
		public async Task<bool> DeleteCartAsync(string id)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var cart = await _unitOfWork.Cart.FindFirstAsync(p => p.Id == id);
			if (cart == null) throw new Exception("Cart not found");
			if (currentUser == null)
				throw new Exception("not allowed to delete");
			await _unitOfWork.Cart.Remove(cart);
			if (await _unitOfWork.SaveAsync() > 0)
			{
				return true;
			}
			return false;
		}
	}
}
