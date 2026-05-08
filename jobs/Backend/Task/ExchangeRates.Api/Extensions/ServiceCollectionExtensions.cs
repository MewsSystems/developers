using ExchangeRates.Application.Options;
using ExchangeRates.Application.Providers;
using ExchangeRates.Infrastructure.Clients.CNB;
using Microsoft.OpenApi.Models;

namespace ExchangeRates.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExchangeRatesServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

            services
                .AddSwaggerDocumentation()
                .AddCorsPolicy()
                .AddCacheSupport()
                .AddAppSettings(configuration)
                .AddCNBClient(configuration)
                .AddApplicationServices();

            return services;
        }

        public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ExchangeRatesOptions>(configuration.GetSection("ExchangeRates"));
            services.Configure<CnbHttpClientOptions>(configuration.GetSection("CNBApi:HttpClient"));
            return services;
        }

        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
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
            return services;
        }

        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
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
            return services;
        }

        public static IServiceCollection AddCacheSupport(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            return services;
        }

        public static IServiceCollection AddCNBClient(this IServiceCollection services, IConfiguration configuration)
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

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IExchangeRatesProvider, ExchangeRatesProvider>();
            return services;
        }
    }
}
