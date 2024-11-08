using AutoMapper;
using E_Commerce.Application.DTO;
using E_Commerce.Application.Helpers;
using E_Commerce.Application.Interfaces;
using E_Commerce.Data.Models;
using E_Commerce.Infrastructure.IGenericRepository_IUOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
	public class ReviewService: IReviewService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserHelpers _userHelpers;
		private readonly IMapper _mapper;
		public ReviewService(IUnitOfWork unitOfWork, IMapper mapper, IUserHelpers userHelpers)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_userHelpers = userHelpers;
		}

		public async Task<ReviewResultDto> GetReviewById(string id)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var review = await _unitOfWork.Reviews.FindFirstAsync(c => c.Id == id, ["Customer","Product"]);
			if (review == null) throw new Exception("Review not found");
			if (currentUser == null) throw new Exception("not allowed to get this Review");
			var result = _mapper.Map<ReviewResultDto>(review);
			return result;
		}
		public async Task<bool> AddReviewAsync(ReviewDto reviewDto)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			if (currentUser == null) throw new Exception("not allowed to add this Review");
			reviewDto.CustomerId = currentUser.Id;

			var review = _mapper.Map<Reviews>(reviewDto);
			await _unitOfWork.Reviews.Add(review);
			if (await _unitOfWork.SaveAsync() > 0)
				return true;
			return false;
		}
		public async Task<IEnumerable<ReviewResultDto>> GetAllReviewsAsync(string productId)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var reviews = await _unitOfWork.Reviews.FindAsync(f => f.ProductId == productId, ["Customer","Product"]);
			if (reviews == null) throw new Exception("Review not found");
			if (currentUser == null) throw new Exception("not allowed to get this Review");
			var result = reviews.Select(_mapper.Map<ReviewResultDto>).ToList();
			return result;
		}
		public async Task<bool> UpdateReviewAsync(string reviewId, ReviewDto reviewDto)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			if (currentUser == null) throw new Exception("not allowed to update");

			var review = await _unitOfWork.Reviews.FindFirstAsync(c => c.Id == reviewId);
			if (review == null) throw new Exception("Review not found");
			reviewDto.CustomerId = currentUser.Id;

			_mapper.Map(reviewDto, review);
			await _unitOfWork.Reviews.UpdateAsync(review);
			if (await _unitOfWork.SaveAsync() > 0)
				return true;
			return false;
		}
		public async Task<bool> DeleteReviewAsync(string id)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var review = await _unitOfWork.Reviews.FindFirstAsync(p => p.Id == id);
			if (review == null) throw new Exception("Review not found");
			if (currentUser == null)
				throw new Exception("not allowed to delete");
			await _unitOfWork.Reviews.Remove(review);
			if (await _unitOfWork.SaveAsync() > 0)
			{
				return true;
			}
			return false;
		}
	}
}
