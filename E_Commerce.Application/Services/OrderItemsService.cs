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
	public class OrderItemsService : IOrderItemsService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserHelpers _userHelpers;
		private readonly IMapper _mapper;
		public OrderItemsService(IUnitOfWork unitOfWork, IMapper mapper, IUserHelpers userHelpers)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_userHelpers = userHelpers;
		}

		public async Task<OrderItemsResultDto> GetOrderItemsByOrderId(string orderId)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var desireListItems = await _unitOfWork.OrderItems.FindAsync(c => c.OrderId == orderId, ["Order", "Product"]);

			if (desireListItems == null || !desireListItems.Any())
				throw new Exception("Order not found");
			if (currentUser == null) throw new Exception("not allowed to get this Order");
			var result = new OrderItemsResultDto
			{
				Order = _mapper.Map<OrderResultDto>(desireListItems.First().Order),
				Products = desireListItems.Select(d => _mapper.Map<ProductResultDto>(d.Product)).ToList()
			};
			return result;
		}

		public async Task<bool> AddItemsToOrderAsync(OrderItemsDto orderItemsDto)
		{
			var orderItem = _mapper.Map<OrderItems>(orderItemsDto);
			await _unitOfWork.OrderItems.Add(orderItem);
			if (await _unitOfWork.SaveAsync() > 0)
				return true;
			return false;
		}

		public async Task<bool> DeleteItemFromOrderAsync(string orderId, string productId)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var orderItem = await _unitOfWork.OrderItems.FindFirstAsync(p => p.OrderId == orderId && p.ProductId == productId);
			if (orderItem == null) throw new Exception("OrderItem not found");
			if (currentUser == null)
				throw new Exception("not allowed to delete");
			await _unitOfWork.OrderItems.Remove(orderItem);
			if (await _unitOfWork.SaveAsync() > 0)
			{
				return true;
			}
			return false;
		}
	}
}
