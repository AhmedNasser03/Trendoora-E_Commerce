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
    public class ProductController : ControllerBase
    {
		private readonly IProductService _productService;
		public ProductController(IProductService productService)
		{
			_productService = productService;
		}

		[Authorize]
		[HttpGet("get-product-by-id")]
		public async Task<IActionResult> GetProductById(string productId)
		{
			var result = await _productService.GetProductById(productId);
			return result != null ? Ok(result) : BadRequest("No Products Found By This Id");
		}

		[Authorize]
		[HttpGet("get-product-by-name")]
		public async Task<IActionResult> GetProductByName(string name)
		{
			var result = await _productService.GetProductByName(name);
			return result != null ? Ok(result) : BadRequest("No Products Found By This Name");
		}

		[Authorize(Roles = UserType.Admin)]
		[HttpGet("get-products")]
		public async Task<IActionResult> GetAllProducts()
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _productService.GetAllProductsAsync();
			return result != null ? Ok(result) : BadRequest("Not Products Founded");
		}
		
		[Authorize(Roles = UserType.Admin)]
		[HttpGet("get-products-by-brand-id")]
		public async Task<IActionResult> GetAllProductsByBrandId(string brandId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _productService.GetAllProductsByBrandId(brandId);
			return result != null ? Ok(result) : BadRequest("Not Products Founded");
		}
		
		[Authorize(Roles = UserType.Admin)]
		[HttpGet("get-products-by-brand-name")]
		public async Task<IActionResult> GetAllProductsByBrandName(string brandName)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _productService.GetAllProductsByBrandName(brandName);
			return result != null ? Ok(result) : BadRequest("Not Products Founded");
		}
		
		[Authorize(Roles = UserType.Admin)]
		[HttpGet("get-products-by-category-id")]
		public async Task<IActionResult> GetAllProductsByCategoryId(string categoryId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _productService.GetAllProductsByCategoryId(categoryId);
			return result != null ? Ok(result) : BadRequest("Not Products Founded");
		}
		
		[Authorize(Roles = UserType.Admin)]
		[HttpGet("get-products-by-category-name")]
		public async Task<IActionResult> GetAllProductsByCategoryName(string categoryName)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _productService.GetAllProductsByCategoryName(categoryName);
			return result != null ? Ok(result) : BadRequest("Not Products Founded");
		}


		[Authorize(Roles = UserType.Admin)]
		[HttpPost("add-product")]
		public async Task<IActionResult> AddProduct(ProductDto productDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _productService.AddProductAsync(productDto);
			return result ? Ok("Product has been Added Successfully") : BadRequest("failed to add Product");
		}

		[Authorize(Roles = UserType.Admin)]
		[HttpPut("update-product")]
		public async Task<IActionResult> UpdateProduct(string productId, ProductDto prodcutDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _productService.UpdateProductAsync(productId, prodcutDto);
			return result ? Ok("Product has been Updated Successfully") : BadRequest("failed to Update Product");
		}

		[Authorize(Roles = UserType.Admin)]
		[HttpDelete("delete-product")]
		public async Task<IActionResult> DeleteBrnad(string productId)
		{
			var result = await _productService.DeleteProductAsync(productId);
			return result ? Ok("Product has been Deleted Successfully") : BadRequest("failed to Delete Product");
		}
	}
}
