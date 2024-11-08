using AutoMapper;
using E_Commerce.Application.Authentication;
using E_Commerce.Application.Helpers;
using E_Commerce.Application.Interfaces;
using E_Commerce.Authentication;
using E_Commerce.Data.Consts;
using E_Commerce.Data.Data;
using E_Commerce.Data.Models;
using E_Commerce.DTO;
using E_Commerce.Mailing;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace E_Commerce.Services
{
	public class AuthorizationRepository : IAuthorizationRepository
	{
		private readonly E_CommerceContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IMapper _mapper;
		private readonly IMailingService _mailingService;
		private readonly IConfiguration _config;
		private readonly IUserHelpers _userHelper;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AuthorizationRepository(UserManager<ApplicationUser> userManager, IMapper mapper, SignInManager<ApplicationUser> signInManager,
			IMailingService mailingService, IConfiguration config, IUserHelpers userHelpers, E_CommerceContext context)
		{
			_userManager = userManager;
			_mapper = mapper;
			_mailingService = mailingService;
			_config = config;
			_userHelper = userHelpers;
			_context = context;
			_signInManager = signInManager;
		}

		public async Task<IdentityResult> RegisterAsync(RegisterUserDto userDto)
		{
			var strategy = _context.Database.CreateExecutionStrategy();

			return await strategy.ExecuteAsync(async () =>
			{
				using (var transaction = await _context.Database.BeginTransactionAsync())
				{
					try
					{
						var currentUser = await _userHelper.GetCurrentUserAsync();
						ApplicationUser userResult = _mapper.Map<ApplicationUser>(userDto);

						// Determine user type and assign accordingly
						if (userDto.Type == "Seller")
						{
							userResult.Type = UserType.Seller;
						}
						if (userDto.Type == "Buyer")
						{
							userResult.Type = UserType.Buyer;
						}

						//var userExist = await _userManager.FindByNameAsync(userDto.UserName);
						var userExist = await _userManager.FindByEmailAsync(userDto.Email);
						if (userExist != null)
						{
							throw new Exception("This User Already Exist");
						}

						IdentityResult result = await _userManager.CreateAsync(userResult, userDto.Password);

						if (result.Succeeded)
						{
							await _userManager.AddToRoleAsync(userResult, userResult.Type);
							userResult.EmailConfirmed = true;
							await _userManager.UpdateAsync(userResult);

							var message = new MailMessage(new string[] { userResult.Email }, "register", $"Hi {userResult.FirstName}, You have been Registered as a {userResult.Type} Successfully In T-ECOM(Trendoor E_Commerce System)");
							_mailingService.SendMail(message);

							await transaction.CommitAsync();

							return result;
						}

						return IdentityResult.Failed(new IdentityError { Description = "Not Allowed To Add Users" });
					}
					catch (Exception)
					{
						await transaction.RollbackAsync();
						throw;
					}
				}
			});
		}
		public async Task<AuthModel> SignInAsync(LoginUser loginUser)
		{
			var authModel = new AuthModel();
			try
			{
				var user = await _userManager.FindByEmailAsync(loginUser.Email);
				if (user == null)
					return new AuthModel { Message = "user not found" };

				if (!await _userManager.CheckPasswordAsync(user, loginUser.Password))
					return new AuthModel { Message = "Invalid password" };

				if (!user.EmailConfirmed)
				{
					return new AuthModel { Message = "user not confirmed" };
				}

				// التحقق من عدد الأجهزة النشطة
				//if (user.ActiveDevices >= 2)
				//{
				//	return new AuthModel { Message = "User is already logged in on two devices" };
				//}

				user.ActiveDevices++;
				await _userManager.UpdateAsync(user);

				authModel.Message = $"Welcome Back, {user.FirstName}";
				authModel.UserName = user.UserName;
				authModel.Email = user.Email;
				authModel.Token = await _userHelper.GenerateJwtTokenAsync(user);

				authModel.IsAuthenticated = true;
				authModel.Roles = [.. (await _userManager.GetRolesAsync(user))];

				if (user.RefreshTokens.Any(a => a.IsActive))
				{
					var ActiveRefreshToken = user.RefreshTokens.First(a => a.IsActive);
					authModel.RefreshToken = ActiveRefreshToken.Token;
					authModel.RefreshTokenExpiration = ActiveRefreshToken.ExpiresOn;
				}
				else
				{
					var refreshToken = GenerateRefreshToken();
					user.RefreshTokens.Add(refreshToken);
					await _userManager.UpdateAsync(user);
					authModel.RefreshToken = refreshToken.Token;
					authModel.RefreshTokenExpiration = refreshToken.ExpiresOn;
				}
				return authModel;
			}
			catch (Exception ex)
			{
				return new AuthModel { Message = "Invalid Authentication", Errors = new List<string> { ex.Message } };
			}
		}

		public async Task<string> LogoutAsync(string token)
		{
			var user = await _userHelper.GetCurrentUserAsync();
			if (user == null)
			{
				return "User Not Found";
			}

			await _signInManager.SignOutAsync();

			user.ActiveDevices = Math.Max(0, user.ActiveDevices - 1);
			await _userManager.UpdateAsync(user);

			await RevokeTokenAsync(token);
			return "User Logged Out Successfully";
		}

		public async Task<AuthModel> RefreshTokenAsync(string Token)
		{
			var user = _userManager.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == Token));
			if (user == null)
			{
				return new AuthModel { Message = "Invalid Token" };
			}
			var refreshToken = user.RefreshTokens.Single(x => x.Token == Token);
			if (!refreshToken.IsActive)
			{
				return new AuthModel { Message = "InActive Token" };
			}
			refreshToken.RevokedOn = DateTime.UtcNow;
			var newRefreshToken = GenerateRefreshToken();
			user.RefreshTokens.Add(newRefreshToken);
			await _userManager.UpdateAsync(user);
			var jwtToken = await _userHelper.GenerateJwtTokenAsync(user);
			return new AuthModel
			{
				Token = jwtToken,
				RefreshToken = newRefreshToken.Token,
				RefreshTokenExpiration = newRefreshToken.ExpiresOn,
				IsAuthenticated = true,
				UserName = user.UserName,
				Email = user.Email,
				Roles = await _userManager.GetRolesAsync(user) as List<string>,
			};
		}

		public async Task<bool> RevokeTokenAsync(string Token)
		{
			var user = _userManager.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == Token));
			if (user == null)
			{
				return false;
			}
			var refreshToken = user.RefreshTokens.Single(x => x.Token == Token);
			if (!refreshToken.IsActive)
			{
				return false;
			}
			refreshToken.RevokedOn = DateTime.UtcNow;
			await _userManager.UpdateAsync(user);
			return true;
		}
		private RefreshToken GenerateRefreshToken()
		{
			var randomNumber = new byte[64];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);
			return new RefreshToken()
			{
				Token = Convert.ToBase64String(randomNumber),
				ExpiresOn = DateTime.UtcNow.AddMinutes(20),
				CreatedOn = DateTime.UtcNow,
			};
		}
		public async Task<IdentityResult> ChangePasswordAsync(ChangePassword changePassword)
		{
			var currentUser = await _userHelper.GetCurrentUserAsync();
			if (currentUser == null)
				return IdentityResult.Failed(new IdentityError { Description = "User not Found" });

			if (currentUser.OTP != changePassword.OTP || currentUser.OTPExpiry < DateTime.UtcNow)
				return IdentityResult.Failed(new IdentityError { Description = "Invalid Or Expired OTP" });

			currentUser.OTP = null;
			currentUser.OTPExpiry = DateTime.MinValue;

			var result = await _userManager.ChangePasswordAsync(currentUser, changePassword.CurrentPassword, changePassword.NewPassword);
			await _userManager.UpdateAsync(currentUser);

			return result;
		}

		public async Task<bool> ForgetPasswordAsync(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
				return false;

			await SendOTPAsync(email);
			return true;
		}

		public async Task<IdentityResult> ResetPasswordAsync(ResetPassword resetPassword)
		{
			var user = await _userManager.FindByEmailAsync(resetPassword.Email);
			if (user == null)
				return IdentityResult.Failed(new IdentityError { Description = "User Not Found" });

			if (user.OTP != resetPassword.OTP || user.OTPExpiry < DateTime.UtcNow)
				return IdentityResult.Failed(new IdentityError { Description = "Invalid Or Expired OTP" });

			user.OTP = null;
			user.OTPExpiry = DateTime.MinValue;

			var token = await _userManager.GeneratePasswordResetTokenAsync(user);
			var result = await _userManager.ResetPasswordAsync(user, token, resetPassword.NewPassword);
			await _userManager.UpdateAsync(user);

			return result;
		}

		public async Task<IdentityResult> SendOTPAsync(string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
				return IdentityResult.Failed(new IdentityError { Description = "User Not Found" });

			var otp = GenerateOTP();
			user.OTP = otp;
			user.OTPExpiry = DateTime.UtcNow.AddMinutes(15);
			await _userManager.UpdateAsync(user);

			var message = new MailMessage(new[] { user.Email }, "Your OTP", $"Your OTP For Change Your Password In T_ECOM is: {otp}");
			_mailingService.SendMail(message);

			return IdentityResult.Success;
		}

		public async Task<IdentityResult> VerifyOTPAsync(VerifyOTPRequest verifyOTPRequest)
		{
			var user = await _userManager.FindByEmailAsync(verifyOTPRequest.Email);
			if (user == null)
				return IdentityResult.Failed(new IdentityError { Description = "User Not Found" });

			if (user.OTP != verifyOTPRequest.OTP || user.OTPExpiry < DateTime.UtcNow)
				return IdentityResult.Failed(new IdentityError { Description = "Invalid Or Expired OTP" });

			user.OTP = string.Empty;
			user.OTPExpiry = DateTime.MinValue;
			user.EmailConfirmed = true;
			await _userManager.UpdateAsync(user);

			return IdentityResult.Success;
		}

		private string GenerateOTP()
		{
			using (var rng = new RNGCryptoServiceProvider())
			{
				var byteArray = new byte[6];
				rng.GetBytes(byteArray);

				var sb = new StringBuilder();
				foreach (var byteValue in byteArray)
				{
					sb.Append(byteValue % 10);
				}
				return sb.ToString();
			}
		}

	}
}
