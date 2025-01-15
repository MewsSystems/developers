using ExchangeRateUpdater.Api.Exceptions;
using ExchangeRateUpdater.Api.Extensions;
using ExchangeRateUpdater.Api.Middleware;
using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Infrastructure;

namespace ExchangeRateUpdater.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;
            var configuration = builder.Configuration;
            var environment = builder.Environment;

            services
                .AddApplicationServices()
                .AddInfrastructureServices(configuration)
                .AddHostServices();

            services.AddSwagger();
            services.AddExceptionHandler<CustomExceptionHandler>();

            using var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseRouting();
            app.MapControllers();

            app.UseSwaggerInApplication();
            app.UseExceptionHandler(options => { });

            app.UseMiddleware<ApiKeyMiddleware>();

            await app.RunAsync();
        }
    }
}
