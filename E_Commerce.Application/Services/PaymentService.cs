using AutoMapper;
using E_Commerce.Application.DTO;
using E_Commerce.Application.Helpers;
using E_Commerce.Application.Interfaces;
using E_Commerce.Data.Consts;
using E_Commerce.Data.Models;
using E_Commerce.Infrastructure.IGenericRepository_IUOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
	public class PaymentService : IPaymentService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserHelpers _userHelpers;
		private readonly IMapper _mapper;
		public PaymentService(IUnitOfWork unitOfWork, IMapper mapper, IUserHelpers userHelpers)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_userHelpers = userHelpers;
		}

		public async Task<PaymentResultDto> GetPaymentById(string id)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var cart = await _unitOfWork.Payment.FindFirstAsync(c => c.Id == id, "Customer");
			if (cart == null) throw new Exception("Payment not found");
			if (currentUser == null) throw new Exception("not allowed to get this Payment");
			var result = _mapper.Map<PaymentResultDto>(cart);
			return result;
		}
		public async Task<bool> AddPaymentAsync(PaymentDto paymentDto)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			if (currentUser == null) throw new Exception("not allowed to add this Payment");
			paymentDto.CustomerId = currentUser.Id;
			var payment = _mapper.Map<Payment>(paymentDto);
			await _unitOfWork.Payment.Add(payment);
			if (await _unitOfWork.SaveAsync() > 0)
				return true;
			return false;
		}
		public async Task<IEnumerable<PaymentResultDto>> GetAllPayemntsAsync()
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var payments = await _unitOfWork.Payment.FindAsync(f => true, "Customer");
			if (payments == null) throw new Exception("Payment not found");
			if (currentUser == null) throw new Exception("not allowed to get this Payment");
			var result = payments.Select(_mapper.Map<PaymentResultDto>).ToList();
			return result;
		}
		public async Task<IEnumerable<string>> GetAllPaymentMethodsAsync()
		{
			return new List<string>
			{
				PaymentMethod.CashUponReceipt,
				PaymentMethod.PayPal,
				PaymentMethod.CreditCard
			};
		}
		public async Task<bool> DeletePaymentAsync(string id)
		{
			var currentUser = await _userHelpers.GetCurrentUserAsync();
			var payment = await _unitOfWork.Payment.FindFirstAsync(p => p.Id == id);
			if (payment == null) throw new Exception("Payment not found");
			if (currentUser == null)
				throw new Exception("not allowed to delete");
			await _unitOfWork.Payment.Remove(payment);
			if (await _unitOfWork.SaveAsync() > 0)
			{
				return true;
			}
			return false;
		}
	}
}
