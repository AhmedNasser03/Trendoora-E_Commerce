using E_Commerce.Application.Interfaces;
using E_Commerce.Authentication;
using E_Commerce.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AuthorizationController : ControllerBase
	{
		private readonly IAuthorizationRepository _authRepository;
		public AuthorizationController(IAuthorizationRepository authRepository)
		{
			_authRepository = authRepository;
		}

		[HttpPost("login")]
		public async Task<IActionResult> signin(LoginUser loginUser)
		{
			var result = await _authRepository.SignInAsync(loginUser);
			if (result == null) return BadRequest("Invalid login Credentials. Please check your Username and Password and Try Again.");
			return Ok(result);
		}

		[HttpPost("register")]
		public async Task<IActionResult> Registration(RegisterUserDto userDto)
		{
			var result = await _authRepository.RegisterAsync(userDto);
			if (!result.Succeeded) return BadRequest("There Was a Problem With This Registration. Please Check The Information You Entered and Try Again.");
			return Ok("Registeration Successfully");
		}

		[HttpPost("LogOut")]
		[Authorize]
		public async Task<IActionResult> Logout(string token)
		{
			var result = await _authRepository.LogoutAsync(token);
			if (result == "User Logged Out Successfully")
				return Ok(result);
			return BadRequest(result);
		}

		[HttpPost("refresh-token")]
		public async Task<IActionResult> RefreshTokenAsync([FromBody] string? refreshToken)
		{
			if (string.IsNullOrEmpty(refreshToken))
				refreshToken = Request.Cookies["refreshToken"];
			if (string.IsNullOrEmpty(refreshToken))
			{
				return BadRequest("Invalid Token");
			}
			var result = await _authRepository.RefreshTokenAsync(refreshToken);
			if (!result.IsAuthenticated)
			{
				return BadRequest(result);
			}
			SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
			return Ok(result);
		}

		[HttpPost("revoke-token")]
		public async Task<IActionResult> RevokeTokenAsync(string? Token)
		{
			Token = Token ?? Request.Cookies["refreshToken"];
			if (string.IsNullOrEmpty(Token))
			{
				return BadRequest("Token is Required");
			}
			var result = await _authRepository.RevokeTokenAsync(Token);
			if (result)
			{
				return Ok("Token Revoked Successfully");
			}
			return BadRequest("Token Not Revoked");
		}

		private void SetRefreshTokenInCookie(string Token, DateTime expires)
		{
			var CoockieOptions = new CookieOptions
			{
				HttpOnly = true,
				Expires = expires.ToLocalTime(),
			};
			Response.Cookies.Append("refreshToken", Token, CoockieOptions);
		}

		[HttpPut("change-password")]
		public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
		{
			var result = await _authRepository.ChangePasswordAsync(changePassword);
			if (!result.Succeeded) return BadRequest(result.Errors);
			return Ok("Your Password Has been Successfully Changed");
		}

		[HttpPost("forget-password")]
		public async Task<IActionResult> ForgetPassword(string email)
		{
			var result = await _authRepository.ForgetPasswordAsync(email);
			return result ? Ok("A password Reset link has been sent to your email Address") : BadRequest("User Not Found To Send OTP for Change Password");
		}

		[HttpPut("reset-password")]
		public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
		{
			var result = await _authRepository.ResetPasswordAsync(resetPassword);
			if (!result.Succeeded) return BadRequest(result.Errors);
			return Ok("Your Password Has been Successfully Reset. You Can Now Login Using Your New Password");
		}

		[HttpPost("send-otp")]
		public async Task<IActionResult> SendOTP(string email)
		{
			var result = await _authRepository.SendOTPAsync(email);
			if (!result.Succeeded) return BadRequest(result.Errors);
			return Ok("A One-Time Passcode (OTP) Has been Sent To Your Email. Please Enter The Code To Continue");
		}

		[HttpPost("verify-otp")]
		public async Task<IActionResult> VerifyOTP([FromBody] VerifyOTPRequest request)
		{
			var result = await _authRepository.VerifyOTPAsync(request);
			if (!result.Succeeded) return BadRequest(result.Errors);
			return Ok("Email Confirmed Successfully");
		}

	}
}
