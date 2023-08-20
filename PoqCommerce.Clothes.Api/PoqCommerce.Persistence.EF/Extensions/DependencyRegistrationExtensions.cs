using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PoqCommerce.Application.Interfaces;
using PoqCommerce.Persistence.EF.Repositories;
using System.Reflection;

namespace PoqCommerce.Persistence.EF.Extensions
{
    public static class DependencyRegistrationExtensions
    {
        public static IServiceCollection RegisterEfPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var assemblyName = typeof(EfDbContext).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<EfDbContext>(
                c => c.UseSqlServer(
                     configuration.GetConnectionString("ProductsConnection"),
                     o => o.MigrationsAssembly(assemblyName)));

            services.AddTransient<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
