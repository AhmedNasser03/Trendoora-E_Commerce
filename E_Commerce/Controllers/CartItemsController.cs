using E_Commerce.Application.DTO;
using E_Commerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class CartItemsController : Controller
	{
		private readonly ICartItemsService _cartItemsService;
		public CartItemsController(ICartItemsService cartItemsService)
		{
			_cartItemsService = cartItemsService;
		}

		[Authorize]
		[HttpGet("get-cartitem-by-id")]
		public async Task<IActionResult> GetCartItemsById(string cartId)
		{
			var result = await _cartItemsService.GetCartItemsByCartId(cartId);
			return result != null ? Ok(result) : BadRequest("No Carts Found By This Id");
		}

		[Authorize]
		[HttpPost("add-item-to-cart")]
		public async Task<IActionResult> AddItemsToCartAsync(CartItemsDto cartItemsDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _cartItemsService.AddItemsToCartAsync(cartItemsDto);
			return result ? Ok("Product has been Added to Cart Successfully") : BadRequest("failed to add Product to Cart");
		}

		[Authorize]
		[HttpDelete("delete-item-from-cart")]
		public async Task<IActionResult> DeleteItemFromCartAsync(string cartId, string productId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _cartItemsService.DeleteItemFromCartAsync(cartId, productId);
			return result ? Ok("Product has been Deleted from cart Successfully") : BadRequest("failed to delete Product from Cart");
		}
	}
}
