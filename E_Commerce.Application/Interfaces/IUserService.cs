using E_Commerce.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces
{
	public interface IUserService
	{
		Task<UserResultDto> GetCurrentUserInfoAsync();
		public Task<bool> EditAccount(EditUserDTO userDto);
		public Task<bool> DeleteAccount();
	}
}
