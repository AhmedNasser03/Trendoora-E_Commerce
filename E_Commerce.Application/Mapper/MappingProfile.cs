using AutoMapper;
using E_Commerce.Application.DTO;
using E_Commerce.Data.Models;
using E_Commerce.DTO;
using System.Net.Mail;
using System.Numerics;

namespace E_Commerce.Mapper
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<RegisterUserDto, ApplicationUser>()
				.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => new MailAddress(src.Email).User));
			CreateMap<UserResultDto, ApplicationUser>().ReverseMap();
			CreateMap<EditUserDTO, ApplicationUser>().ReverseMap();
			CreateMap<BrandDto, Brand>().ReverseMap();
			CreateMap<BrandResultDto, Brand>().ReverseMap();
			CreateMap<CategoryDto, Category>().ReverseMap();
			CreateMap<CategoryResultDto, Category>().ReverseMap();
			CreateMap<ProductDto, Product>().ReverseMap();
			CreateMap<ProductResultDto, Product>().ReverseMap();
			CreateMap<CartDto, Cart>().ReverseMap();
			CreateMap<Cart, CartResultDto>()
				.ForMember(dest => dest.Customer, opt => opt.MapFrom(src => new UserResultDto
				{
					FirstName = src.Customer.FirstName,
					LastName = src.Customer.LastName,
					PhoneNumber = src.Customer.PhoneNumber,
					Type = src.Customer.Type
				}))
				  .ForMember(dest => dest.Coupon, opt => opt.MapFrom(src => src.Coupon))
				  .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
				  .ReverseMap()
				  .ForPath(dest => dest.Customer.FirstName, opt => opt.MapFrom(src => src.Customer.FirstName))
				  .ForPath(dest => dest.Customer.LastName, opt => opt.MapFrom(src => src.Customer.LastName))
				  .ForPath(dest => dest.Customer.PhoneNumber, opt => opt.MapFrom(src => src.Customer.PhoneNumber))
				  .ForPath(dest => dest.Customer.Type, opt => opt.MapFrom(src => src.Customer.Type));

			CreateMap<OrderDto, Order>().ReverseMap();
			CreateMap<Order, OrderResultDto>()
				  .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => new UserResultDto
				  {
					  FirstName = src.Customer.FirstName,
					  LastName = src.Customer.LastName,
					  PhoneNumber = src.Customer.PhoneNumber,
					  Type = src.Customer.Type
				  }))
				  .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
				  .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress))
				  .ForMember(dest => dest.ShippingMethod, opt => opt.MapFrom(src => src.ShippingMethod))
				  .ForMember(dest => dest.ShippingDate, opt => opt.MapFrom(src => src.ShippingDate))
				  .ForMember(dest => dest.DeliveringDate, opt => opt.MapFrom(src => src.DeliveringDate))
				  .ReverseMap()
				  .ForPath(dest => dest.Customer.FirstName, opt => opt.MapFrom(src => src.Customer.FirstName))
				  .ForPath(dest => dest.Customer.LastName, opt => opt.MapFrom(src => src.Customer.LastName))
				  .ForPath(dest => dest.Customer.PhoneNumber, opt => opt.MapFrom(src => src.Customer.PhoneNumber))
				  .ForPath(dest => dest.Customer.Type, opt => opt.MapFrom(src => src.Customer.Type));
			CreateMap<ReviewDto, Reviews>().ReverseMap();
			CreateMap<Reviews, ReviewResultDto>()
				.ForMember(dest => dest.Customer, opt => opt.MapFrom(src => new UserResultDto
				{
					FirstName = src.Customer.FirstName,
					LastName = src.Customer.LastName,
					PhoneNumber = src.Customer.PhoneNumber,
					Type = src.Customer.Type
				}))
				.ForMember(dest => dest.Product, opt => opt.MapFrom(src => new ProductResultDto
				{
					Name = src.Product.Name,
					Price = src.Product.Price,
					Description = src.Product.Description
				}))
				  .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate))
				  .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
				  .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
				  .ReverseMap()
				  .ForPath(dest => dest.Customer.FirstName, opt => opt.MapFrom(src => src.Customer.FirstName))
				  .ForPath(dest => dest.Customer.LastName, opt => opt.MapFrom(src => src.Customer.LastName))
				  .ForPath(dest => dest.Customer.PhoneNumber, opt => opt.MapFrom(src => src.Customer.PhoneNumber))
				  .ForPath(dest => dest.Customer.Type, opt => opt.MapFrom(src => src.Customer.Type))
				  .ForPath(dest => dest.Product.Name, opt => opt.MapFrom(src => src.Product.Name))
				  .ForPath(dest => dest.Product.Price, opt => opt.MapFrom(src => src.Product.Price))
				  .ForPath(dest => dest.Product.Description, opt => opt.MapFrom(src => src.Product.Description));

			CreateMap<PaymentDto, Payment>().ReverseMap();
			CreateMap<Payment, PaymentResultDto>()
				.ForMember(dest => dest.Customer, opt => opt.MapFrom(src => new UserResultDto
				{
					FirstName = src.Customer.FirstName,
					LastName = src.Customer.LastName,
					PhoneNumber = src.Customer.PhoneNumber,
					Type = src.Customer.Type
				}))
				  .ForMember(dest => dest.Method, opt => opt.MapFrom(src => src.Method))
				  .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
				  .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
				  .ReverseMap()
				  .ForPath(dest => dest.Customer.FirstName, opt => opt.MapFrom(src => src.Customer.FirstName))
				  .ForPath(dest => dest.Customer.LastName, opt => opt.MapFrom(src => src.Customer.LastName))
				  .ForPath(dest => dest.Customer.PhoneNumber, opt => opt.MapFrom(src => src.Customer.PhoneNumber))
				  .ForPath(dest => dest.Customer.Type, opt => opt.MapFrom(src => src.Customer.Type));

			CreateMap<DesireListDto, DesireList>().ReverseMap();
			CreateMap<DesireList, DesireListResultDto>()
				.ForMember(dest => dest.Customer, opt => opt.MapFrom(src => new UserResultDto
				{
					FirstName = src.Customer.FirstName,
					LastName = src.Customer.LastName,
					PhoneNumber = src.Customer.PhoneNumber,
					Type = src.Customer.Type
				}))
				  .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				  .ReverseMap()
				  .ForPath(dest => dest.Customer.FirstName, opt => opt.MapFrom(src => src.Customer.FirstName))
				  .ForPath(dest => dest.Customer.LastName, opt => opt.MapFrom(src => src.Customer.LastName))
				  .ForPath(dest => dest.Customer.PhoneNumber, opt => opt.MapFrom(src => src.Customer.PhoneNumber))
				  .ForPath(dest => dest.Customer.Type, opt => opt.MapFrom(src => src.Customer.Type));
			CreateMap<DesireListItemDto, DesireListItems>().ReverseMap();
			CreateMap<DesireListItems, DesireListItemResultDto>()
				.ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.DesireList.DesireListItems.Select(d => d.Product)))
				.ReverseMap();

			CreateMap<CartItemsDto, CartItems>().ReverseMap();
			CreateMap<CartItems, CartItemsResultDto>()
				.ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Cart.CartItems.Select(d => d.Product)))
				.ReverseMap();

			CreateMap<OrderItemsDto, OrderItems>().ReverseMap();
			CreateMap<OrderItems, OrderItemsResultDto>()
				.ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Order.OrderItems.Select(d => d.Product)))
				.ReverseMap();
		}

	}
}
