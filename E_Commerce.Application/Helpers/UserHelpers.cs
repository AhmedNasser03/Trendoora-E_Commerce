using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using E_Commerce.Data.Consts;
using System;
using E_Commerce.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using E_Commerce.Data.Models;

namespace E_Commerce.Application.Helpers
{
	public class UserHelpers : IUserHelpers
	{
		#region fields
		private IWebHostEnvironment _webHostEnvironment;
		private readonly IConfiguration _config;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IHttpContextAccessor _contextAccessor;
		#endregion

		#region ctor
		public UserHelpers(IConfiguration config, UserManager<ApplicationUser> userManager
			, IHttpContextAccessor contextAccessor
			, IWebHostEnvironment webHostEnvironment)
		{
			_config = config;
			_userManager = userManager;
			_contextAccessor = contextAccessor;
			_webHostEnvironment = webHostEnvironment;
		}
		#endregion


		public async Task<ApplicationUser> GetCurrentUserAsync()
		{
			var currentUser = _contextAccessor.HttpContext.User;
			return await _userManager.GetUserAsync(currentUser);
		}

		public async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
		{
			var claims = new List<Claim>
			{
				new(ClaimTypes.Email, user.Email),
				new(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var roles = await _userManager.GetRolesAsync(user);
			claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
			var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var tokenExpiration = DateTime.Now.AddDays(1);
			var token = new JwtSecurityToken(
				issuer: _config["JWT:ValidIssuer"],
				audience: _config["JWT:ValidAudience"],
				claims: claims,
				expires: tokenExpiration,
				signingCredentials: signingCredentials
			);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		#region file handling
		public async Task<string> AddFileAsync(IFormFile file, string folderName)
		{
			if (file == null || file.Length == 0)
			{
				return string.Empty;
			}

			string rootPath = _webHostEnvironment.WebRootPath;
			var user = await GetCurrentUserAsync();
			string userName = user.UserName;
			string profileFolderPath = "";
			if (folderName == UserType.Buyer)
				profileFolderPath = Path.Combine(rootPath, "Buyer", userName);
			else
				profileFolderPath = Path.Combine(rootPath, "Images", userName, folderName);
			if (!Directory.Exists(profileFolderPath))
			{
				Directory.CreateDirectory(profileFolderPath);
			}

			string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
			string filePath = Path.Combine(profileFolderPath, fileName);

			using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(fileStream);
			}
			if (folderName == UserType.Seller)
				return $"/Seller/{userName}/{fileName}";
			return $"/Images/{userName}/{folderName}/{fileName}";

		}

		public async Task<bool> DeleteFileAsync(string filePath, string folderName)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				return true;
			}

			string rootPath = _webHostEnvironment.WebRootPath;
			var user = await GetCurrentUserAsync();
			string userName = user.UserName;

			if (folderName == UserType.Buyer)
			{
				if (!filePath.StartsWith($"/Buyer/{userName}/"))
				{
					throw new ArgumentException("Invalid file path.", nameof(filePath));
				}
			}

			else
			{
				if (!filePath.StartsWith($"/Images/{userName}/{folderName}/"))
				{
					throw new ArgumentException("Invalid file path.", nameof(filePath));
				}
			}
			string fullFilePath = Path.Combine(rootPath, filePath.TrimStart('/'));

			if (File.Exists(fullFilePath))
			{
				File.Delete(fullFilePath);
				return true;
			}
			else
			{
				throw new FileNotFoundException("File not found.", fullFilePath);
			}

		}
		#endregion

	}
}