using E_Commerce.Application.DTO;
using E_Commerce.Application.Helpers;
using E_Commerce.Application.Interfaces;
using E_Commerce.Application.Services;
using E_Commerce.Data.Consts;
using E_Commerce.Data.Models;
using E_Commerce.Infrastructure.IGenericRepository_IUOW;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryService _categoryService;
		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[Authorize]
		[HttpGet("get-category-by-id")]
		public async Task<IActionResult> GetCategoryById(string categoryId)
		{
			var result = await _categoryService.GetCategoryById(categoryId);
			return result != null ? Ok(result) : BadRequest("No Categories Found By This Id");
		}

		[Authorize]
		[HttpGet("get-category-by-name")]
		public async Task<IActionResult> GetCategoryByName(string name)
		{
			var result = await _categoryService.GetCategoryByName(name);
			return result != null ? Ok(result) : BadRequest("No Categories Found By This Name");
		}

		[Authorize(Roles = UserType.Admin)]
		[HttpGet("get-categories")]
		public async Task<IActionResult> GetAllCategories()
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _categoryService.GetAllCategoriesAsync();
			return result != null ? Ok(result) : BadRequest("Not Categories Founded");
		}

		[Authorize(Roles = UserType.Admin)]
		[HttpPost("add-category")]
		public async Task<IActionResult> AddCategory(CategoryDto categoryDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _categoryService.AddCategoryAsync(categoryDto);
			return result ? Ok("Category has been Added Successfully") : BadRequest("failed to add Category");
		}



		[Authorize(Roles = UserType.Admin)]
		[HttpPut("update-category")]
		public async Task<IActionResult> UpdateCategory(string categoryId, CategoryDto categoryDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _categoryService.UpdateCategoryAsync(categoryId, categoryDto);
			return result ? Ok("Category has been Updated Successfully") : BadRequest("failed to Update Category");
		}

		[Authorize(Roles = UserType.Admin)]
		[HttpDelete("delete-category")]
		public async Task<IActionResult> DeleteCategory(string categoryId)
		{
			var result = await _categoryService.DeleteCategoryAsync(categoryId);
			return result ? Ok("Category has been Deleted Successfully") : BadRequest("failed to Delete Category");
		}

	}
}
