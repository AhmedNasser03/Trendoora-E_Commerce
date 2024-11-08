using AutoMapper;
using E_Commerce.Application.DTO;
using E_Commerce.Application.Helpers;
using E_Commerce.Application.Interfaces;
using E_Commerce.Data.Models;
using E_Commerce.Infrastructure.GenericRepository_UOW;
using E_Commerce.Infrastructure.IGenericRepository_IUOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserHelpers _userHelpers;
		private readonly IMapper _mapper;
		public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IUserHelpers userHelpers)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_userHelpers = userHelpers;
		}

		public async Task<CategoryResultDto> GetCategoryById(string id)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var category = await _unitOfWork.Category.FindFirstAsync(c => c.Id == id, ["Products"]);
			if (category == null) throw new Exception("Category not found");
			if (currentUser == null) throw new Exception("not allowed to get this category");
			var result = _mapper.Map<CategoryResultDto>(category);
			return result;
		}
		public async Task<CategoryResultDto> GetCategoryByName(string name)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var category = await _unitOfWork.Category.FindFirstAsync(c => c.Name == name, ["Products"]);
			if (category == null) throw new Exception("Category not found");
			if (currentUser == null) throw new Exception("not allowed to get this category");
			var result = _mapper.Map<CategoryResultDto>(category);
			return result;
		}

		public async Task<bool> AddCategoryAsync(CategoryDto categoryDto)
		{
			var isExist = await _unitOfWork.Category.FindFirstAsync(f => f.Name == categoryDto.Name);
			if (isExist != null) throw new Exception("This Category is Already Exist");
			var category = _mapper.Map<Category>(categoryDto);
			await _unitOfWork.Category.Add(category);
			if (await _unitOfWork.SaveAsync() > 0)
				return true;
			return false;
		}
		public async Task<IEnumerable<CategoryResultDto>> GetAllCategoriesAsync()
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var categories = await _unitOfWork.Category.FindAsync(f=> true, "Products");
			if (categories == null) throw new Exception("Category not found");
			if (currentUser == null) throw new Exception("not allowed to get this category");
			var result = categories.Select(_mapper.Map<CategoryResultDto>).ToList();
			return result;
		}
		public async Task<bool> UpdateCategoryAsync(string categoryId, CategoryDto categoryDto)
		{
			var isExist = await _unitOfWork.Category.FindFirstAsync(f => f.Name == categoryDto.Name);
			if (isExist != null) throw new Exception("This Category is Already Exist");

			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var category = await _unitOfWork.Category.FindFirstAsync(c => c.Id == categoryId);
			if (category == null) throw new Exception("Category not found");
			if (currentUser == null) throw new Exception("not allowed to update");

			_mapper.Map(categoryDto, category);
			await _unitOfWork.Category.UpdateAsync(category);
			if (await _unitOfWork.SaveAsync() > 0)
				return true;
			return false;
		}
		public async Task<bool> DeleteCategoryAsync(string id)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var category = await _unitOfWork.Category.FindFirstAsync(p => p.Id == id);
			if (category == null) throw new Exception("Category not found");
			if (currentUser == null)
				throw new Exception("not allowed to delete");
			await _unitOfWork.Category.Remove(category);
			if (await _unitOfWork.SaveAsync() > 0)
			{
				return true;
			}
			return false;
		}
	}
}
