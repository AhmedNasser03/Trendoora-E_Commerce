using E_Commerce.Application.DTO;

namespace E_Commerce.Application.Interfaces
{
	public interface IBrandService
	{
		Task<BrandResultDto> GetBrandById(string id);
		Task<BrandResultDto> GetBrandByName(string name);
		Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();
		Task<bool> UpdateBrandAsync(string brandId, BrandDto brandDto);	
		Task<bool> AddBrandAsync(BrandDto brandDto);
		Task<bool> DeleteBrandAsync(string id);
	}
}
