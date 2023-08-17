using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PoqCommerce.Application.Interfaces;

namespace PoqCommerce.Application.Extensions
{
    public static class DependencyRegistration
    {
        public static IServiceCollection RegisterApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IProductService, ProductService>();

            return services;
        }
    }
}
