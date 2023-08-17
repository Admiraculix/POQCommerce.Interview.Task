
using AspNetCoreRateLimit;
using AutoMapper.EquivalencyExpression;
using PoqCommerce.Api.AutoMapper.Profiles;
using PoqCommerce.Application.Extensions;
using PoqCommerce.Mocky.Io.Extensions;
using Serilog;
using System.Reflection;

namespace PoqCommerce.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.RegisterApplicationDependencies(builder.Configuration);
            builder.Services.RegisterMockyIoClients(builder.Configuration);

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine("Logs", "log.txt"), rollingInterval: RollingInterval.Day)
                .CreateLogger();
            builder.Logging.AddSerilog();

            // Request Rate limiting
            builder.Services.AddMemoryCache();
            builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
            builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
            builder.Services.AddInMemoryRateLimiting();
            builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            // Register AutoMapper
            builder.Services.AddAutoMapper(
                 (cfg) =>
                 {
                     cfg.AddCollectionMappers();
                 },
                 typeof(RequestToDtoProfile).GetTypeInfo().Assembly);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            app.UseIpRateLimiting();

            app.Run();
        }

    }
}