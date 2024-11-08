using E_Commerce.Application.DTO;
using E_Commerce.Application.Interfaces;
using E_Commerce.Data.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class BrandController : ControllerBase
	{
		private readonly IBrandService _brandService;
		public BrandController(IBrandService brandService)
		{
			_brandService = brandService;
		}

		[Authorize(Roles = UserType.Admin)]
		[HttpGet("get-brand-by-id")]
		public async Task<IActionResult> GetBrandById(string brandId)
		{
			var result = await _brandService.GetBrandById(brandId);
			return result != null ? Ok(result) : BadRequest("No Brands Found By This Id");
		}

		[Authorize(Roles = UserType.Admin)]
		[HttpGet("get-brand-by-name")]
		public async Task<IActionResult> GetBrandByName(string name)
		{
			var result = await _brandService.GetBrandByName(name);
			return result != null ? Ok(result) : BadRequest("No Brands Found By This Name");
		}

		[Authorize(Roles = UserType.Admin)]
		[HttpGet("get-brands")]
		public async Task<IActionResult> GetAllCategories()
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _brandService.GetAllBrandsAsync();
			return result != null ? Ok(result) : BadRequest("Not Brands Founded");
		}

		[Authorize(Roles = UserType.Admin)]
		[HttpPost("add-brand")]
		public async Task<IActionResult> AddCategory(BrandDto brandDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _brandService.AddBrandAsync(brandDto);
			return result ? Ok("Brand has been Added Successfully") : BadRequest("failed to add Brand");
		}

		[Authorize(Roles = UserType.Admin)]
		[HttpPut("update-brand")]
		public async Task<IActionResult> UpdateCategory(string brandId, BrandDto brandDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _brandService.UpdateBrandAsync(brandId, brandDto);
			return result ? Ok("Brand has been Updated Successfully") : BadRequest("failed to Update Brand");
		}

		[Authorize(Roles = UserType.Admin)]
		[HttpDelete("delete-brand")]
		public async Task<IActionResult> DeleteBrand(string brandId)
		{
			var result = await _brandService.DeleteBrandAsync(brandId);
			return result ? Ok("Brand has been Deleted Successfully") : BadRequest("failed to Delete Brand");
		}

	}
}
