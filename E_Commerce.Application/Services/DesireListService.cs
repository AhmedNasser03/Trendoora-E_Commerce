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
	public class DesireListService : IDesireListService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserHelpers _userHelpers;
		private readonly IMapper _mapper;
		public DesireListService(IUnitOfWork unitOfWork, IMapper mapper, IUserHelpers userHelpers)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_userHelpers = userHelpers;
		}

		public async Task<DesireListResultDto> GetDesireListById(string id)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var desireList = await _unitOfWork.DesireList.FindFirstAsync(c => c.Id == id, "Customer");
			if (desireList == null) throw new Exception("DesireList not found");
			if (currentUser == null) throw new Exception("not allowed to get this DesireList");
			var result = _mapper.Map<DesireListResultDto>(desireList);
			return result;
		}
		public async Task<bool> AddDesireListAsync(DesireListDto desireListDto)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			if (currentUser == null) throw new Exception("not allowed to add this DesireList");
			desireListDto.CustomerId = currentUser.Id;

			var desireList = _mapper.Map<DesireList>(desireListDto);
			await _unitOfWork.DesireList.Add(desireList);
			if (await _unitOfWork.SaveAsync() > 0)
				return true;
			return false;
		}
		public async Task<IEnumerable<DesireListResultDto>> GetAllDesireListsAsync()
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var desireLists = await _unitOfWork.DesireList.FindAsync(f => true , "Customer");
			if (desireLists == null) throw new Exception("DesireList not found");
			if (currentUser == null) throw new Exception("not allowed to get this DesireList");
			var result = desireLists.Select(_mapper.Map<DesireListResultDto>).ToList();
			return result;
		}
		public async Task<bool> UpdateDesireListAsync(string desireListId, DesireListDto desireListDto)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			if (currentUser == null) throw new Exception("not allowed to update");

			var desireList = await _unitOfWork.DesireList.FindFirstAsync(c => c.Id == desireListId);
			if (desireList == null) throw new Exception("DesireList not found");
			desireListDto.CustomerId = currentUser.Id;

			_mapper.Map(desireListDto, desireList);
			await _unitOfWork.DesireList.UpdateAsync(desireList);
			if (await _unitOfWork.SaveAsync() > 0)
				return true;
			return false;
		}
		public async Task<bool> DeleteDesireListAsync(string id)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var desireList = await _unitOfWork.DesireList.FindFirstAsync(p => p.Id == id);
			if (desireList == null) throw new Exception("DesireList not found");
			if (currentUser == null)
				throw new Exception("not allowed to delete");
			await _unitOfWork.DesireList.Remove(desireList);
			if (await _unitOfWork.SaveAsync() > 0)
			{
				return true;
			}
			return false;
		}
	}
}
