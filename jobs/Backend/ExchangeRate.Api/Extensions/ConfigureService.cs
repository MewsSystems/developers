using ExchangeRate.Infrastructure.ExternalServices.Builders;
using ExchangeRate.Infrastructure.ExternalServices.Configs;
using ExchangeRate.Infrastructure.ExternalServices.CzechNationalBank;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;

namespace ExchangeRate.Api.Extensions
{
    public static class ConfigureService
    {
        public static void ConfigureApiServices(this WebApplicationBuilder builder) 
        {
            ConfigureCors(builder.Services);
            builder.Services.Configure<JsonOptions>(o => o.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));
            builder.Services.AddControllers().AddNewtonsoftJson( options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            builder.Services.AddEndpointsApiExplorer();

            ConfigureSwagger(builder.Services);

            Load(builder.Services, builder.Configuration);

        }

        private static void Load(IServiceCollection services, ConfigurationManager configuration)
        {
            IInfraConfigs infra = new InfraConfigs();
            configuration.GetSection("InfraConfigs").Bind(infra);
            services.AddSingleton<IInfraConfigs>(infra);

            services.AddScoped<IExchangeRatesService, ExchangeRatesService>();
            services.AddScoped<IBuildExchangeRates, BuildExchangeRates>();
        }

        private static void ConfigureCors(IServiceCollection services)
        {
            services.AddCors(x => x.AddPolicy("MyAllowSpecificOrigins", builder =>
            {
                builder.WithOrigins("*")
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .Build();
            }));
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddMemoryCache();

            services.AddSwaggerGen(swgOptions =>
            {
                swgOptions.CustomSchemaIds( type => type.Name);
                swgOptions.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Exchange Rate Api Czech National Bank",
                    Version = "v1",
                    Description = "Provider based on real world public data source of Czech National Bank",
                });
            });


        }
    }

}
