using AutoMapper;
using E_Commerce.Application.DTO;
using E_Commerce.Application.Helpers;
using E_Commerce.Application.Interfaces;
using E_Commerce.Data.Models;
using E_Commerce.Infrastructure.IGenericRepository_IUOW;

namespace E_Commerce.Application.Services
{
	public class BrandService : IBrandService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserHelpers _userHelpers;
		private readonly IMapper _mapper;
		public BrandService(IUnitOfWork unitOfWork, IMapper mapper, IUserHelpers userHelpers)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_userHelpers = userHelpers;
		}

		public async Task<BrandResultDto> GetBrandById(string id)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var brand = await _unitOfWork.Brand.FindFirstAsync(c => c.Id == id, ["Products"]);
			if (brand == null) throw new Exception("Brand not found");
			if (currentUser == null) throw new Exception("not allowed to get this Brand");
			var result = _mapper.Map<BrandResultDto>(brand);
			return result;
		}
		public async Task<BrandResultDto> GetBrandByName(string name)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var brand = await _unitOfWork.Brand.FindFirstAsync(c => c.Name == name, ["Products"]);
			if (brand == null) throw new Exception("Brand not found");
			if (currentUser == null) throw new Exception("not allowed to get this Brand");
			var result = _mapper.Map<BrandResultDto>(brand);
			return result;
		}
		public async Task<bool> AddBrandAsync(BrandDto brandDto)
		{
			var isExist = await _unitOfWork.Brand.FindFirstAsync(f => f.Name == brandDto.Name);
			if (isExist != null) throw new Exception("This Brand is Already Exist");
			var brand = _mapper.Map<Brand>(brandDto);
			await _unitOfWork.Brand.Add(brand);
			if (await _unitOfWork.SaveAsync() > 0)
				return true;
			return false;
		}
		public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var brands = await _unitOfWork.Brand.FindAsync(f => true, "Products");
			if (brands == null) throw new Exception("Brand not found");
			if (currentUser == null) throw new Exception("not allowed to get this Brand");
			var result = brands.Select(_mapper.Map<BrandResultDto>).ToList();
			return result;
		}
		public async Task<bool> UpdateBrandAsync(string categoryId, BrandDto brandDto)
		{
			var isExist = await _unitOfWork.Brand.FindFirstAsync(f => f.Name == brandDto.Name);
			if (isExist != null) throw new Exception("This Brand is Already Exist");

			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var brand = await _unitOfWork.Brand.FindFirstAsync(c => c.Id == categoryId);
			if (brand == null) throw new Exception("Brand not found");
			if (currentUser == null) throw new Exception("not allowed to update");

			_mapper.Map(brandDto, brand);
			await _unitOfWork.Brand.UpdateAsync(brand);
			if (await _unitOfWork.SaveAsync() > 0)
				return true;
			return false;
		}
		public async Task<bool> DeleteBrandAsync(string id)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var brand = await _unitOfWork.Brand.FindFirstAsync(p => p.Id == id);
			if (brand == null) throw new Exception("Brand not found");
			if (currentUser == null)
				throw new Exception("not allowed to delete");
			await _unitOfWork.Brand.Remove(brand);
			if (await _unitOfWork.SaveAsync() > 0)
			{
				return true;
			}
			return false;
		}
	}
}
