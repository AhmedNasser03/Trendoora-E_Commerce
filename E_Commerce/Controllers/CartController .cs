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
    public class CartController : ControllerBase
    {
		private readonly ICartService _cartService;
		public CartController(ICartService cartService)
		{
			_cartService = cartService;
		}

		[Authorize]
		[HttpGet("get-cart-by-id")]
		public async Task<IActionResult> GetCartById(string cartId)
		{
			var result = await _cartService.GetCartById(cartId);
			return result != null ? Ok(result) : BadRequest("No Carts Found By This Id");
		}

		[Authorize]
		[HttpGet("get-carts")]
		public async Task<IActionResult> GetAllCarts()
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _cartService.GetAllCartsAsync();
			return result != null ? Ok(result) : BadRequest("Not Carts Founded");
		}

		[Authorize]
		[HttpPost("add-cart")]
		public async Task<IActionResult> AddCart(CartDto cartDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _cartService.AddCartAsync(cartDto);
			return result ? Ok("Cart has been Added Successfully") : BadRequest("failed to add Cart");
		}

		[Authorize]
		[HttpPut("update-cart")]
		public async Task<IActionResult> UpdateCart(string cartId, CartDto cartDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _cartService.UpdateCartAsync(cartId, cartDto);
			return result ? Ok("Cart has been Updated Successfully") : BadRequest("failed to Update Cart");
		}

		[Authorize]
		[HttpDelete("delete-cart")]
		public async Task<IActionResult> DeleteCart(string cartId)
		{
			var result = await _cartService.DeleteCartAsync(cartId);
			return result ? Ok("Cart has been Deleted Successfully") : BadRequest("failed to Delete Cart");
		}
	}
}
