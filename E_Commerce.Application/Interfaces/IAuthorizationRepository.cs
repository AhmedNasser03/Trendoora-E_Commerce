using E_Commerce.Application.Authentication;
using E_Commerce.Application.DTO;
using E_Commerce.Authentication;
using E_Commerce.DTO;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Application.Interfaces
{
	public interface IAuthorizationRepository
	{
		Task<IdentityResult> RegisterAsync(RegisterUserDto userDto);
		public Task<AuthModel> SignInAsync(LoginUser loginUser);
		public Task<string> LogoutAsync(string token);
		Task<bool> ForgetPasswordAsync(string email);
		Task<IdentityResult> ResetPasswordAsync(ResetPassword resetPassword);
		Task<IdentityResult> ChangePasswordAsync(ChangePassword changePassword);
		Task<IdentityResult> VerifyOTPAsync(VerifyOTPRequest request);
		Task<IdentityResult> SendOTPAsync(string email);
		public Task<AuthModel> RefreshTokenAsync(string Token);
		public Task<bool> RevokeTokenAsync(string Token);
	}
}
