using E_Commerce.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces
{
	public interface IProductService
	{
		Task<ProductResultDto> GetProductById(string id);
		Task<ProductResultDto> GetProductByName(string name);
		Task<bool> AddProductAsync(ProductDto productDto);
		Task<IEnumerable<ProductResultDto>> GetAllProductsAsync();
		Task<IEnumerable<ProductResultDto>> GetAllProductsByBrandId(string brandId);
		Task<IEnumerable<ProductResultDto>> GetAllProductsByBrandName(string brandName);	
		Task<IEnumerable<ProductResultDto>> GetAllProductsByCategoryId(string categoryId);
		Task<IEnumerable<ProductResultDto>> GetAllProductsByCategoryName(string categoryName);
		Task<bool> UpdateProductAsync(string produtcId, ProductDto productDto);
		Task<bool> DeleteProductAsync(string id);
	}
}
