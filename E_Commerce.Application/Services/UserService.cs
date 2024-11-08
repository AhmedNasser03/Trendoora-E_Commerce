using AutoMapper;
using E_Commerce.Application.DTO;
using E_Commerce.Application.Helpers;
using E_Commerce.Application.Interfaces;
using E_Commerce.Data.Data;
using E_Commerce.Data.Models;
using E_Commerce.Infrastructure.GenericRepository_UOW;
using E_Commerce.Infrastructure.IGenericRepository_IUOW;
using E_Commerce.Mailing;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
	public class UserService : IUserService
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserHelpers _userHelper;
        public UserService(IUserHelpers userHelper , IMapper mapper , IUnitOfWork unitOfWork)
        {
            _userHelper = userHelper;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
        }


        public async Task<UserResultDto> GetCurrentUserInfoAsync()
		{
			var currentUser = await _userHelper.GetCurrentUserAsync() ?? throw new Exception("User not found.");
			var result = _mapper.Map<UserResultDto>(currentUser);
			return result;
		}

		public async Task<bool> EditAccount(EditUserDTO userDto)
		{
			var currentUser = await _userHelper.GetCurrentUserAsync() ?? throw new Exception("User not found");
			try
			{
				currentUser = _mapper.Map(userDto, currentUser);
				await _unitOfWork.User.UpdateAsync(currentUser);
				await _unitOfWork.SaveAsync();
			}
			catch
			{
				return false;
			}
			return true;
		}
		public async Task<bool> DeleteAccount()
		{
			var currentUser = await _userHelper.GetCurrentUserAsync() ?? throw new Exception("User not found");

			try
			{
				await _unitOfWork.User.Remove(currentUser);
				await _unitOfWork.SaveAsync();
			}
			catch
			{
				return false;
			}
			return true;
		}
	}
}
