using E_Commerce.Application.DTO;
using E_Commerce.Application.Interfaces;
using E_Commerce.Application.Services;
using E_Commerce.Data.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
		private readonly IOrderService _orderService;
		public OrderController(IOrderService orderService)
		{
			_orderService = orderService;
		}

		[Authorize]
		[HttpGet("get-order-by-id")]
		public async Task<IActionResult> GetOrderById(string orderId)
		{
			var result = await _orderService.GetOrderById(orderId);
			return result != null ? Ok(result) : BadRequest("No Orders Found By This Id");
		}

		[Authorize]
		[HttpGet("get-orders")]
		public async Task<IActionResult> GetAllOrders()
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _orderService.GetAllOrdersAsync();
			return result != null ? Ok(result) : BadRequest("Not Orders Founded");
		}

		[Authorize]
		[HttpPost("add-order")]
		public async Task<IActionResult> AddOrder(OrderDto orderDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _orderService.AddOrderAsync(orderDto);
			return result ? Ok("Order has been Added Successfully") : BadRequest("failed to add Order");
		}

		[Authorize]
		[HttpPut("update-order")]
		public async Task<IActionResult> UpdateOrder(string orderId, OrderDto orderDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _orderService.UpdateOrderAsync(orderId, orderDto);
			return result ? Ok("Order has been Updated Successfully") : BadRequest("failed to Update Order");
		}

		[Authorize]
		[HttpGet("shipping-methods")]
		public async Task<ActionResult<IEnumerable<string>>> GetAllPaymentMethods()
		{
			var methods = await _orderService.GetAllShippingMethodsAsync();
			return Ok(methods);
		}

		[Authorize]
		[HttpDelete("delete-order")]
		public async Task<IActionResult> DeleteOrder(string orderId)
		{
			var result = await _orderService.DeleteOrderAsync(orderId);
			return result ? Ok("Order has been Deleted Successfully") : BadRequest("failed to Delete Order");
		}
	}
}
