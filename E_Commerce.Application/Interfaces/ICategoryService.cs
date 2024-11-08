using E_Commerce.Application.DTO;
using E_Commerce.Infrastructure.IGenericRepository_IUOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces
{
	public interface ICategoryService
	{
		Task<CategoryResultDto> GetCategoryById(string id);
		Task<CategoryResultDto> GetCategoryByName(string name);
		Task<bool> AddCategoryAsync(CategoryDto categoryDto);
		Task<IEnumerable<CategoryResultDto>> GetAllCategoriesAsync();
		Task<bool> UpdateCategoryAsync(string categoryId ,CategoryDto categoryDto);
		Task<bool> DeleteCategoryAsync(string id);
	}
}
