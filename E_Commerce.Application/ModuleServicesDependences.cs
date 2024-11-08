using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using E_Commerce.Mailing;
using E_Commerce.Application.Helpers;
using E_Commerce.Application.Interfaces;
using E_Commerce.Services;
using E_Commerce.Mapper;
using E_Commerce.Application.Services;
using E_Commerce.Infrastructure.IGenericRepository_IUOW;
using E_Commerce.Infrastructure.GenericRepository_UOW;

namespace E_Commerce.Application
{
    public static class ModuleServicesDependences
    {
        public static IServiceCollection AddReposetoriesServices(this IServiceCollection service)
        {
            service.AddAutoMapper(typeof(MappingProfile));
			service.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
            service.AddScoped<IUserHelpers, UserHelpers>();
            service.AddScoped<IUserService, UserService>();
			service.AddScoped<ICategoryService,CategoryService>();
			service.AddScoped<IBrandService, BrandService>();
			service.AddScoped<IProductService, ProductService>();
            service.AddScoped<IOrderService, OrderService>();
            service.AddScoped<IPaymentService,PaymentService>();
            service.AddScoped<IReviewService, ReviewService>();
            service.AddScoped<ICartService, CartService>();
			service.AddScoped<IDesireListService, DesireListService>();
			service.AddScoped<IDesireListItemsService, DesireListItemsService>();
			service.AddScoped<IOrderItemsService, OrderItemsService>();
			service.AddScoped<ICartItemsService, CartItemsService>();
			service.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            service.AddScoped<IUnitOfWork, UnitOfWork>();
			service.AddScoped<IMailingService, MailingService>();
            service.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return service;
        }
    }
}
