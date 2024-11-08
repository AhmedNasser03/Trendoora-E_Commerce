using E_Commerce.Application.DTO;
using E_Commerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class OrderItemsController : Controller
	{
		private readonly IOrderItemsService _orderItemsService;
		public OrderItemsController(IOrderItemsService orderItemsService)
		{
			_orderItemsService = orderItemsService;
		}

		[Authorize]
		[HttpGet("get-orderitem-by-id")]
		public async Task<IActionResult> GetCartItemsById(string orderId)
		{
			var result = await _orderItemsService.GetOrderItemsByOrderId(orderId);
			return result != null ? Ok(result) : BadRequest("No Orders Found By This Id");
		}

		[Authorize]
		[HttpPost("add-item-to-order")]
		public async Task<IActionResult> AddItmesToCartAsync(OrderItemsDto orderItemsDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _orderItemsService.AddItemsToOrderAsync(orderItemsDto);
			return result ? Ok("Product has been Added to Order Successfully") : BadRequest("failed to add Product to Order");
		}

		[Authorize]
		[HttpDelete("delete-item-from-order")]
		public async Task<IActionResult> DeleteItemFromCartAsync(string orderId, string productId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _orderItemsService.DeleteItemFromOrderAsync(orderId, productId);
			return result ? Ok("Product has been Deleted from Order Successfully") : BadRequest("failed to delete Product from Order");
		}
	}
}
