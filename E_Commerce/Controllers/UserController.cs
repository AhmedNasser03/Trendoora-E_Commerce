using E_Commerce.Application.DTO;
using E_Commerce.Application.Helpers;
using E_Commerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IUserHelpers _userHelper;
		public UserController(IUserService userService , IUserHelpers userHelper)
		{
			_userService = userService;
			_userHelper = userHelper;
		}

		[Authorize]
		[HttpGet("getcurrentuser")]
		public async Task<IActionResult> GetCurrentUserInfo()
		{
			var result = await _userService.GetCurrentUserInfoAsync();
			return result != null ? Ok(result) : BadRequest("user not found");
		}

		[Authorize]
		[HttpPut("updateaccount")]
		public async Task<IActionResult> EditCurrentUserInfo(EditUserDTO userDTO)
		{
			var result = await _userService.EditAccount(userDTO);
			return result ? Ok("account has been updated successfully") : BadRequest("user not found");
		}

		[Authorize]
		[HttpDelete("deleteaccount")]
		public async Task<IActionResult> DeleteCurrentUser()
		{
			var result = await _userService.DeleteAccount();
			return result ? Ok("account has been deleted successfully") : BadRequest("user not found");
		}
	}
}
