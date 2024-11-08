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
	public class ProductService : IProductService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserHelpers _userHelpers;
		private readonly IMapper _mapper;
		public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IUserHelpers userHelpers)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_userHelpers = userHelpers;
		}

		public async Task<ProductResultDto> GetProductById(string id)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var product = await _unitOfWork.Product.FindFirstAsync(c => c.Id == id);
			if (product == null) throw new Exception("Product not found");
			if (currentUser == null) throw new Exception("not allowed to get this product");
			var result = _mapper.Map<ProductResultDto>(product);
			return result;
		}
		public async Task<ProductResultDto> GetProductByName(string name)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var product = await _unitOfWork.Product.FindFirstAsync(c => c.Name == name);
			if (product == null) throw new Exception("Product not found");
			if (currentUser == null) throw new Exception("not allowed to get this product");
			var result = _mapper.Map<ProductResultDto>(product);
			return result;
		}

		public async Task<bool> AddProductAsync(ProductDto productDto)
		{
			var isExist = await _unitOfWork.Product.FindFirstAsync(f => f.Name == productDto.Name);
			if (isExist != null) throw new Exception("This Product is Already Exist");
			var product = _mapper.Map<Product>(productDto);
			await _unitOfWork.Product.Add(product);
			if (await _unitOfWork.SaveAsync() > 0)
				return true;
			return false;
		}
		public async Task<IEnumerable<ProductResultDto>> GetAllProductsAsync()
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var products = await _unitOfWork.Product.FindAsync(f => true);
			if (products == null) throw new Exception("Product not found");
			if (currentUser == null) throw new Exception("not allowed to get products");
			var result = products.Select(_mapper.Map<ProductResultDto>).ToList();
			return result;
		}
		public async Task<IEnumerable<ProductResultDto>> GetAllProductsByBrandId(string brandId)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var products = await _unitOfWork.Product.FindAsync(p=>p.BrandId == brandId);
			if (products == null || !products.Any()) throw new Exception("Products not found");
			if (currentUser == null) throw new Exception("not allowed to get products");
			var result = products.Select(_mapper.Map<ProductResultDto>).ToList();
			return result;
		}
		public async Task<IEnumerable<ProductResultDto>> GetAllProductsByBrandName(string brandName)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var products = await _unitOfWork.Product.FindAsync(p => p.Brand != null && p.Brand.Name == brandName);
			if (products == null || !products.Any()) throw new Exception("Products not found");
			if (currentUser == null) throw new Exception("not allowed to get products");
			var result = products.Select(_mapper.Map<ProductResultDto>).ToList();
			return result;
		}
		public async Task<IEnumerable<ProductResultDto>> GetAllProductsByCategoryId(string categoryId)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var products = await _unitOfWork.Product.FindAsync(p => p.CategoryId == categoryId);
			if (products == null || !products.Any()) throw new Exception("Products not found");
			if (currentUser == null) throw new Exception("not allowed to get products");
			var result = products.Select(_mapper.Map<ProductResultDto>).ToList();
			return result;
		}
		public async Task<IEnumerable<ProductResultDto>> GetAllProductsByCategoryName(string categoryName)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var products = await _unitOfWork.Product.FindAsync(p=>p.Category !=null && p.Category.Name == categoryName);
			if (products == null || !products.Any()) throw new Exception("Products not found");
			if (currentUser == null) throw new Exception("not allowed to get products");
			var result = products.Select(_mapper.Map<ProductResultDto>).ToList();
			return result;
		}
		public async Task<bool> UpdateProductAsync(string productId, ProductDto productDto)
		{
			var isExist = await _unitOfWork.Product.FindFirstAsync(f => f.Name == productDto.Name);
			if (isExist != null) throw new Exception("This Category is Already Exist");

			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var product = await _unitOfWork.Product.FindFirstAsync(c => c.Id == productId);
			if (product == null) throw new Exception("Product not found");
			if (currentUser == null) throw new Exception("not allowed to update");

			_mapper.Map(productDto, product);
			await _unitOfWork.Product.UpdateAsync(product);
			if (await _unitOfWork.SaveAsync() > 0)
				return true;
			return false;
		}
		public async Task<bool> DeleteProductAsync(string id)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var product = await _unitOfWork.Product.FindFirstAsync(p => p.Id == id);
			if (product == null) throw new Exception("Product not found");
			if (currentUser == null)
				throw new Exception("not allowed to delete");
			await _unitOfWork.Product.Remove(product);
			if (await _unitOfWork.SaveAsync() > 0)
			{
				return true;
			}
			return false;
		}
	}
}
