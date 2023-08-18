using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PoqCommerce.Application.Interfaces;
using PoqCommerce.Persistence.EF.Repositories;

namespace PoqCommerce.Persistence.EF.Extensions
{
    public static class DependencyRegistrationExtensions
    {
        public static IServiceCollection RegisterEfPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
