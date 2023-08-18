
using AspNetCoreRateLimit;
using AutoMapper.EquivalencyExpression;
using FluentValidation.AspNetCore;
using PoqCommerce.Api.AutoMapper.Profiles;
using PoqCommerce.Api.Validators;
using PoqCommerce.Application.Extensions;
using PoqCommerce.Mocky.Io.Extensions;
using PoqCommerce.Persistence.Extensions;
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
            builder.Services.RegisterPersistenceDependencies(builder.Configuration);

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
            builder.Services.AddInMemoryRateLimiting();
            builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            // Register FluentValition
            builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<FilterObjectValidator>());

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