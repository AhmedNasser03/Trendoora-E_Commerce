using E_Commerce.Authentication;
using E_Commerce.Data.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace E_Commerce.Application.Helpers
{
    public interface IUserHelpers
    {
        Task<ApplicationUser> GetCurrentUserAsync();
        Task<string> AddFileAsync(IFormFile file, string folderName);
        Task<bool> DeleteFileAsync(string imagePath, string folderName);
		Task<string> GenerateJwtTokenAsync(ApplicationUser user);

	}
}
