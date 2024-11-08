using AutoMapper;
using E_Commerce.Application.DTO;
using E_Commerce.Application.Helpers;
using E_Commerce.Application.Interfaces;
using E_Commerce.Data.Consts;
using E_Commerce.Data.Models;
using E_Commerce.Infrastructure.IGenericRepository_IUOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
	public class OrderService : IOrderService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserHelpers _userHelpers;
		private readonly IMapper _mapper;
		public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IUserHelpers userHelpers)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_userHelpers = userHelpers;
		}

		public async Task<OrderResultDto> GetOrderById(string id)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var order = await _unitOfWork.Order.FindFirstAsync(c => c.Id == id, "Customer");
			if (order == null) throw new Exception("Order not found");
			if (currentUser == null) throw new Exception("not allowed to get this Order");
			var result = _mapper.Map<OrderResultDto>(order);
			return result;
		}
		public async Task<bool> AddOrderAsync(OrderDto orderDto)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			if (currentUser == null) throw new Exception("not allowed to update");
			orderDto.CustomerId = currentUser.Id;

			var order = _mapper.Map<Order>(orderDto);
			await _unitOfWork.Order.Add(order);
			if (await _unitOfWork.SaveAsync() > 0)
				return true;
			return false;
		}
		public async Task<IEnumerable<OrderResultDto>> GetAllOrdersAsync()
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var orders = await _unitOfWork.Order.FindAsync(f => true, "Customer");
			if (orders == null) throw new Exception("Order not found");
			if (currentUser == null) throw new Exception("not allowed to get this Order");
			var result = orders.Select(_mapper.Map<OrderResultDto>).ToList();
			return result;
		}
		public async Task<bool> UpdateOrderAsync(string orderId, OrderDto orderDto)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			if (currentUser == null) throw new Exception("not allowed to update");
			orderDto.CustomerId = currentUser.Id;
			var order = await _unitOfWork.Order.FindFirstAsync(c => c.Id == orderId);
			if (order == null) throw new Exception("Order not found");

			_mapper.Map(orderDto, order);
			await _unitOfWork.Order.UpdateAsync(order);
			if (await _unitOfWork.SaveAsync() > 0)
				return true;
			return false;
		}
		public async Task<IEnumerable<string>> GetAllShippingMethodsAsync()
		{
			return new List<string>
			{
				BuyingMethod.Shipping,
				BuyingMethod.BookInStore
			};
		}
		public async Task<bool> DeleteOrderAsync(string id)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var order = await _unitOfWork.Order.FindFirstAsync(p => p.Id == id);
			if (order == null) throw new Exception("Order not found");
			if (currentUser == null)
				throw new Exception("not allowed to delete");
			await _unitOfWork.Order.Remove(order);
			if (await _unitOfWork.SaveAsync() > 0)
			{
				return true;
			}
			return false;
		}
	}
}
