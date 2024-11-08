using E_Commerce.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces
{
	public interface IReviewService
	{
		Task<ReviewResultDto> GetReviewById(string id);
		Task<bool> AddReviewAsync(ReviewDto reviewDto);
		Task<IEnumerable<ReviewResultDto>> GetAllReviewsAsync(string productId);
		Task<bool> UpdateReviewAsync(string reviewId, ReviewDto reviewDto);
		Task<bool> DeleteReviewAsync(string id);
	}
}
