using ExchangeRates.Application.Options;
using ExchangeRates.Application.Providers;
using ExchangeRates.Infrastructure.Clients.CNB;
using Microsoft.OpenApi.Models;

namespace ExchangeRates.Api
{
    public static class Configuration
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            AddControllers(services);
            AddSwagger(services);
            AddCors(services);
            AddCache(services);
            AddAppSettings(services, configuration);
            AddCNBClient(services, configuration);
            AddServices(services);
        }

        private static void AddAppSettings(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ExchangeRatesOptions>(configuration.GetSection("ExchangeRates"));
            services.Configure<CnbHttpClientOptions>(configuration.GetSection("CNBApi:HttpClient"));
        }

        private static void AddControllers(IServiceCollection services)
        {
            services.AddControllers();
        }

        private static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Exchange Rates API",
                    Description = "API that provides daily exchange rates from the Czech National Bank."
                });
            });
        }


        private static void AddCors(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
        }

        private static void AddCache(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
        }

        private static void AddCNBClient(IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration
                .GetSection("CNBApi:HttpClient")
                .Get<CnbHttpClientOptions>()
                ?? new CnbHttpClientOptions();

            if (options == null)
                throw new InvalidOperationException("CNBApi configuration section is missing.");

            if (string.IsNullOrWhiteSpace(options.BaseUrl))
                throw new InvalidOperationException("CNBApi:HttpClient:BaseUrl configuration value is missing.");

            services.AddHttpClient<ICnbHttpClient, CnbHttpClient>((serviceProvider, client) =>
            {
                client.BaseAddress = new Uri(options.BaseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddPolicyHandler((serviceProvider, request) =>
            {
                return CnbHttpClientPolicies.TimeoutPolicy(options);
            })
            .AddPolicyHandler((serviceProvider, request) =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<CnbHttpClient>>();
                return CnbHttpClientPolicies.RetryPolicy(options, logger);
            });
        }


        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IExchangeRatesProvider, ExchangeRatesProvider>();
        }

        public static void Configure(WebApplication app, IHostEnvironment env)
        {
            app.UseExceptionHandler("/error");
            app.UseCors("AllowAll");
            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Exchange Rates API v1");
                c.RoutePrefix = "swagger"; // mantiene la ruta /swagger/index.html
            });

            app.MapControllers();
            app.Map("/error", (HttpContext httpContext) =>
            {
                return Results.Problem("An unexpected error occurred. Please try again later.");
            });
        }
    }
}
