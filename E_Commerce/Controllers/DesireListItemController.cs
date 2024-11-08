using E_Commerce.Application.DTO;
using E_Commerce.Application.Interfaces;
using E_Commerce.Data.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class DesireListItemController : Controller
	{
		private readonly IDesireListItemsService _desireListItemsService;
		public DesireListItemController(IDesireListItemsService desireListItemsService)
		{
			_desireListItemsService = desireListItemsService;
		}

		[Authorize]
		[HttpGet("get-desirelistitem-by-id")]
		public async Task<IActionResult> GetDesireListItemsById(string desireListId)
		{
			var result = await _desireListItemsService.GetDesireListItemsById(desireListId);
			return result != null ? Ok(result) : BadRequest("No DesireLists Found By This Id");
		}

		[Authorize]
		[HttpPost("add-desireListitem")]
		public async Task<IActionResult> AddItmesToDesireListAsync(string desireListId, string productId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _desireListItemsService.AddItemsToDesireListAsync(desireListId , productId);
			return result ? Ok("Product has been Added to DesireList Successfully") : BadRequest("failed to add Product to Desirelist");
		}

		[Authorize]
		[HttpDelete("delete-desireListitem")]
		public async Task<IActionResult> DeleteItemFromDesireListAsync(string desireListId, string productId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _desireListItemsService.DeleteItemFromDesireListAsync(desireListId, productId);
			return result ? Ok("Product has been Deleted from DesireList Successfully") : BadRequest("failed to delete Product from DesireList");
		}
	}
}
