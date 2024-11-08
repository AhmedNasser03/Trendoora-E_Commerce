using Microsoft.Extensions.DependencyInjection;
using E_Commerce.Infrastructure.GenericRepository_UOW;
using E_Commerce.Infrastructure.IGenericRepository_IUOW;

namespace E_Commerce.Infrastructure
{
    public static class ModuleInfrastructureDependences
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection service)
        {
            service.AddTransient<IUnitOfWork, UnitOfWork>();
            return service;
        }
    }
}
