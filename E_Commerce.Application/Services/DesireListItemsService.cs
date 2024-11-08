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
	public class DesireListItemsService : IDesireListItemsService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserHelpers _userHelpers;
		private readonly IMapper _mapper;
		public DesireListItemsService(IUnitOfWork unitOfWork, IMapper mapper, IUserHelpers userHelpers)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_userHelpers = userHelpers;
		}

		public async Task<DesireListItemResultDto> GetDesireListItemsById(string desireListId)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var desireListItems = await _unitOfWork.DesireListItems.FindAsync(c => c.DesireListId == desireListId, ["DesireList","Product"]);

			if (desireListItems == null || !desireListItems.Any())
				throw new Exception("DesireList not found"); 
			if (currentUser == null) throw new Exception("not allowed to get this DesireList");
			var result = new DesireListItemResultDto
			{
				DesireList = _mapper.Map<DesireListResultDto>(desireListItems.First().DesireList),
				Products = desireListItems.Select(d => _mapper.Map<ProductResultDto>(d.Product)).ToList()
			}; 
			return result;
		}

		public async Task<bool> AddItemsToDesireListAsync(string desireListId, string productId)
		{
			var desireListItem = new DesireListItems
			{
				DesireListId = desireListId,
				ProductId = productId
			};

			await _unitOfWork.DesireListItems.Add(desireListItem);
			if (await _unitOfWork.SaveAsync() > 0)
				return true;
			return false;
		}

		public async Task<bool> DeleteItemFromDesireListAsync(string desireListId, string productId)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var desireListItem = await _unitOfWork.DesireListItems.FindFirstAsync(p => p.DesireListId == desireListId && p.ProductId == productId);
			if (desireListItem == null) throw new Exception("DesireListItem not found");
			if (currentUser == null)
				throw new Exception("not allowed to delete");
			await _unitOfWork.DesireListItems.Remove(desireListItem);
			if (await _unitOfWork.SaveAsync() > 0)
			{
				return true;
			}
			return false;
		}
	}
}
