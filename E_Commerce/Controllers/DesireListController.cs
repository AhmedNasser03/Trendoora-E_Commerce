using E_Commerce.Application.DTO;
using E_Commerce.Application.Interfaces;
using E_Commerce.Data.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class DesireListController : Controller
	{

		private readonly IDesireListService _desireListService;
		public DesireListController(IDesireListService desireListService)
		{
			_desireListService = desireListService;
		}

		[Authorize]
		[HttpGet("get-desirelist-by-id")]
		public async Task<IActionResult> GetReviewById(string desireListId)
		{
			var result = await _desireListService.GetDesireListById(desireListId);
			return result != null ? Ok(result) : BadRequest("No DesireLists Found By This Id");
		}

		[Authorize(Roles = UserType.Admin)]
		[HttpGet("get-desireLists")]
		public async Task<IActionResult> GetAlldesireLists()
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _desireListService.GetAllDesireListsAsync();
			return result != null ? Ok(result) : BadRequest("Not DesireLists Founded");
		}

		[Authorize]
		[HttpPost("add-desireList")]
		public async Task<IActionResult> AddDesireList(DesireListDto desireListDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _desireListService.AddDesireListAsync(desireListDto);
			return result ? Ok("DesireList has been Added Successfully") : BadRequest("failed to add DesireList");
		}

		[Authorize]
		[HttpPut("update-desireList")]
		public async Task<IActionResult> UpdateDesireList(string desireListId, DesireListDto desireListDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _desireListService.UpdateDesireListAsync(desireListId, desireListDto);
			return result ? Ok("DesireList has been Updated Successfully") : BadRequest("failed to Update DesireList");
		}

		[Authorize]
		[HttpDelete("delete-desireList")]
		public async Task<IActionResult> DeleteDesireList(string desireListId)
		{
			var result = await _desireListService.DeleteDesireListAsync(desireListId);
			return result ? Ok("DesireList has been Deleted Successfully") : BadRequest("failed to Delete DesireList");
		}
	}
}
