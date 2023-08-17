using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PoqCommerce.Application.Interfaces;
using PoqCommerce.Mocky.Io.Configurations;

namespace PoqCommerce.Mocky.Io.Extensions
{
    public static class DependencyRegistration
    {
        public static IServiceCollection RegisterMockyIoClients(this IServiceCollection services, IConfiguration configuration)
        {
             var mockyClientCfg = configuration.GetSection("MockyClient")
                .Get<MockyClientConfiguration>(options => options.BindNonPublicProperties = true);
            services.AddSingleton(mockyClientCfg);

            var mockyHttpClientCfg = GetMockyClientConfiguringAction(mockyClientCfg);
            services.AddHttpClient<IMockyHttpClient, MockyHttpClient>(mockyHttpClientCfg);

            return services;
        }

        private static Action<HttpClient> GetMockyClientConfiguringAction(MockyClientConfiguration clientCfg) =>
    (HttpClient client) =>
    {
        client.BaseAddress = new Uri(clientCfg.BaseUrl);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    };
    }
}
