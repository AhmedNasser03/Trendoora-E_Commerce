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
    public class ReviewController : ControllerBase
    {
		private readonly IReviewService _reviewService;
		public ReviewController(IReviewService reviewService)
		{
			_reviewService = reviewService;
		}

		[Authorize]
		[HttpGet("get-review-by-id")]
		public async Task<IActionResult> GetReviewById(string reviewId)
		{
			var result = await _reviewService.GetReviewById(reviewId);
			return result != null ? Ok(result) : BadRequest("No Reviews Found By This Id");
		}

		[Authorize(Roles = UserType.Admin)]
		[HttpGet("get-reviews")]
		public async Task<IActionResult> GetAllReviews(string productId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _reviewService.GetAllReviewsAsync(productId);
			return result != null ? Ok(result) : BadRequest("Not Reviews Founded for this Product");
		}

		[Authorize]
		[HttpPost("add-review")]
		public async Task<IActionResult> AddReview(ReviewDto reviewDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _reviewService.AddReviewAsync(reviewDto);
			return result ? Ok("Review has been Added Successfully") : BadRequest("failed to add Review");
		}

		[Authorize]
		[HttpPut("update-review")]
		public async Task<IActionResult> UpdateRreview(string reviewId, ReviewDto reviewDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var result = await _reviewService.UpdateReviewAsync(reviewId, reviewDto);
			return result ? Ok("Review has been Updated Successfully") : BadRequest("failed to Update Review");
		}

		[Authorize]
		[HttpDelete("delete-review")]
		public async Task<IActionResult> DeleteOrder(string reviewId)
		{
			var result = await _reviewService.DeleteReviewAsync(reviewId);
			return result ? Ok("Review has been Deleted Successfully") : BadRequest("failed to Delete Review");
		}
	}
}
