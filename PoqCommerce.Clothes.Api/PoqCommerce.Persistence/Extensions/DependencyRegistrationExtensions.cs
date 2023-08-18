using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PoqCommerce.Application.Interfaces;
using PoqCommerce.Persistence.EF.Extensions;

namespace PoqCommerce.Persistence.Extensions
{
    public static class DependencyRegistrationExtensions
    {
        public static IServiceCollection RegisterPersistenceDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterEfPersistence(configuration);
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
