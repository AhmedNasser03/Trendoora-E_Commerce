using E_Commerce.Application.DTO;
using E_Commerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
		private readonly IPaymentService _paymentService;
		public PaymentController(IPaymentService paymentService)
		{
			_paymentService = paymentService;
		}

		[Authorize]
		[HttpGet("get-payment-by-id")]
		public async Task<IActionResult> GetPaymentById(string paymentId)
		{
			var result = await _paymentService.GetPaymentById(paymentId);
			return result != null ? Ok(result) : BadRequest("No Payments Found By This Id");
		}

		[Authorize]
		[HttpGet("get-payments")]
		public async Task<IActionResult> GetAllPayments()
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _paymentService.GetAllPayemntsAsync();
			return result != null ? Ok(result) : BadRequest("Not Payments Founded");
		}

		[Authorize]
		[HttpPost("add-payment")]
		public async Task<IActionResult> AddCart(PaymentDto paymentDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _paymentService.AddPaymentAsync(paymentDto);
			return result ? Ok("Payment has been Added Successfully") : BadRequest("failed to add Payment");
		}

		[Authorize]
		[HttpGet("payment-methods")]
		public async Task<ActionResult<IEnumerable<string>>> GetAllPaymentMethods()
		{
			var methods = await _paymentService.GetAllPaymentMethodsAsync();
			return Ok(methods);
		}

		[Authorize]
		[HttpDelete("delete-payment")]
		public async Task<IActionResult> DeletePayment(string paymentId)
		{
			var result = await _paymentService.DeletePaymentAsync(paymentId);
			return result ? Ok("Payment has been Deleted Successfully") : BadRequest("failed to Delete Payment");
		}

	}
}
